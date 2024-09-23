using Invoices.Data.Models.Enums;

namespace Invoices.Data.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public CurrencyType CurrencyType { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; } = null!;
    }
}
