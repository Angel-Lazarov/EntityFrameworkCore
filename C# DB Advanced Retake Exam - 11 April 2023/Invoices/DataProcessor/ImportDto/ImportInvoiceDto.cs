using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        [Required]
        [Range(InvoiceNumberMinValue, InvoiceNumberMaxValue)]
        public int Number { get; set; }

        [Required]
        public string IssueDate { get; set; } = null!; //Deserialize DateTime as string !!! for Datetime.tryParse validation!!!

        [Required]
        public string DueDate { get; set; } = null!; //Deserialize DateTime as string !!! for DateTime.TryParse validation!!!

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Range(InvoiceCurrencyTypeMinValue, InvoiceCurrencyTypeMaxValue)]
        public int CurrencyType { get; set; }  //deserialize Enums as int, to make range validation. In Dto -> cast int to EnumType!!!

        [Required]
        public int ClientId { get; set; }
    }
}
