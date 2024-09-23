namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            Sales = new HashSet<Sale>();
        }
        public int CustomerId { get; set; }


        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string CreditCardNumber { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
