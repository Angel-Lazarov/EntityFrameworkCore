namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        public Product()
        {
            Sales = new HashSet<Sale>();
        }

        public int ProductId { get; set; }

        //[MaxLength(50)]
        public string Name { get; set; } = null!;

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public ICollection<Sale> Sales { get; set; }

    }
}
