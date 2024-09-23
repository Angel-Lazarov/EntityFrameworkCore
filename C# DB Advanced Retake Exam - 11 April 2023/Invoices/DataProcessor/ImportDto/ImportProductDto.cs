
using Newtonsoft.Json;

namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;


    using static Data.DataConstraints;


    public class ImportProductDto
    {
        [Required]
        [MinLength(ProductNameMinLength)]
        [MaxLength(ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), ProductPriceMinValue, ProductPriceMaxValue)]
        //[Range(5.00, 1000.00)]
        public decimal Price { get; set; }

        [Required]
        [Range(ProductCategoryTypeMinValue, ProductCategoryTypeMaxValue)]
        public int CategoryType { get; set; }

        [Required]
        public int[] Clients { get; set; } = null!;
    }
}
