using Invoices.DataProcessor.ExportDto;
using Invoices.Utilities;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Invoices.DataProcessor
{
    using Invoices.Data;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            StringBuilder sb = new();

            XmlHelper helper = new XmlHelper();

            ClientDto[] clients = context.Clients
                .Where(c => c.Invoices.Any(i => i.IssueDate > date))

                .Select(c => new ClientDto()
                {
                    InvoicesCount = c.Invoices.Count,
                    Name = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                        .OrderBy(i => i.IssueDate)
                        .ThenByDescending(i => i.DueDate)
                        .Select(i => new InvoiceDto()
                        {
                            Number = i.Number,
                            Amount = i.Amount,
                            DueDate = i.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            CurrencyType = i.CurrencyType.ToString()
                        })
                        .ToArray()
                })
                .OrderByDescending(c => c.InvoicesCount)
                .ThenBy(c => c.Name)
                .ToArray();

            return helper.Serialize(clients, "Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var validProducts = context.Products
                .Where(p => p.ProductsClients.Count > 0)
                .Where(p => p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
                .Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients
                        .Where(pc => pc.Client.Name.Length >= nameLength)
                        .Select(c => new
                        {
                            Name = c.Client.Name,
                            NumberVat = c.Client.NumberVat
                        })
                        .OrderBy(c => c.Name)
                        .ToArray()
                })
                .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            string result = JsonConvert.SerializeObject(validProducts, Formatting.Indented);

            return result;

        }
    }
}