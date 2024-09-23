using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;
namespace Invoices.Data.Models
{
    public class Client
    {
        public int Id { get; set; }

        [MaxLength(ClientNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(ClientNumberVatMaxLength)]
        public string NumberVat { get; set; } = null!;

        public virtual ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();

        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();

        public ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();
    }
}
