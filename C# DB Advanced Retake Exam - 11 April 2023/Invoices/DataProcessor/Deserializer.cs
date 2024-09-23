using Invoices.Data.Models;
using Invoices.Data.Models.Enums;
using Invoices.DataProcessor.ImportDto;
using Invoices.Utilities;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Invoices.DataProcessor
{
    using Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();
            string xmlRoot = "Clients";
            StringBuilder sb = new();

            var exportedDtos = helper.Deserialize<ImportClientDto[]>(xmlString, xmlRoot);
            var clientsToImport = new List<Client>();

            foreach (var clientDto in exportedDtos)
            {
                var addressToImport = new List<Address>();

                if (!IsValid(clientDto))
                {
                    sb.AppendLine(string.Format(ErrorMessage));
                    continue;
                }

                foreach (var dtoAddress in clientDto.Addresses)
                {
                    if (!IsValid(dtoAddress))
                    {
                        sb.AppendLine(string.Format(ErrorMessage));
                        continue;
                    }

                    Address newAddress = new Address()
                    {
                        StreetName = dtoAddress.StreetName,
                        StreetNumber = dtoAddress.StreetNumber,
                        PostCode = dtoAddress.PostCode,
                        City = dtoAddress.City,
                        Country = dtoAddress.Country
                    };

                    addressToImport.Add(newAddress);
                }

                Client newClient = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                    Addresses = addressToImport
                };
                clientsToImport.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, newClient.Name));
            }

            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            var deserializedInvoices = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString);
            StringBuilder sb = new();

            var invoicesToImport = new List<Invoice>();

            foreach (var invoiceDto in deserializedInvoices)
            {
                if (!IsValid(invoiceDto))
                {
                    sb.AppendLine(string.Format(ErrorMessage));
                    continue;
                }

                bool isIssueDateValid = DateTime.TryParse(invoiceDto.IssueDate, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime issueDate);

                bool isDueDateValid = DateTime.TryParse(invoiceDto.DueDate, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime dueDate);

                //validate Dates !!!
                if (isIssueDateValid == false || isDueDateValid == false || DateTime.Compare(dueDate, issueDate) < 0)
                {
                    sb.AppendLine(string.Format(ErrorMessage));
                    continue;
                }

                //validate Client
                if (!context.Clients.Any(c => c.Id == invoiceDto.ClientId))
                {
                    sb.AppendLine(string.Format(ErrorMessage));
                    continue;
                }

                Invoice newInvoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = issueDate, //tryParsed Date
                    DueDate = dueDate, //tryParsed Date
                    Amount = invoiceDto.Amount,
                    CurrencyType = (CurrencyType)invoiceDto.CurrencyType, //cast int to EnumType
                    ClientId = invoiceDto.ClientId
                };

                invoicesToImport.Add(newInvoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoiceDto.Number));
            }

            context.Invoices.AddRange(invoicesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new();

            var productsToImport = new List<Product>();

            var deserializedProducts = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);



            foreach (ImportProductDto productDto in deserializedProducts)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product newProduct = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = (CategoryType)productDto.CategoryType
                };

                var productClientsToImport = new List<ProductClient>();

                foreach (int clientId in productDto.Clients.Distinct()) // only Unique Ids !!!
                {
                    if (!context.Clients.Any(c => c.Id == clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient productClient = new ProductClient()
                    {
                        ClientId = clientId,
                        Product = newProduct
                    };

                    productClientsToImport.Add(productClient);
                }

                newProduct.ProductsClients = productClientsToImport;

                productsToImport.Add(newProduct);

                sb.AppendLine(
                    string.Format(SuccessfullyImportedProducts, newProduct.Name, productClientsToImport.Count));
            }

            context.Products.AddRange(productsToImport);
            context.SaveChanges();

            return sb.ToString();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
