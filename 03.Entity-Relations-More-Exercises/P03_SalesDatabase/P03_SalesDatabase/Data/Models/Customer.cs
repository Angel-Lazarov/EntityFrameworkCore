namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            Sales = new HashSet<Sale>();
        }
        public int CustomerId { get; set; }

        //[MaxLength(100)]
        public string Name { get; set; } = null!;

        //[MaxLength(80)]
        //[Column(TypeName = "VARCHAR")]
        public string? Email { get; set; }

        //[MaxLength(20)]
        public string CreditCardNumber { get; set; } = null!;

        public ICollection<Sale> Sales { get; set; }

    }
}
