using Invoices.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;

namespace Invoices.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public CategoryType CategoryType { get; set; }

        public virtual ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();
    }
}
