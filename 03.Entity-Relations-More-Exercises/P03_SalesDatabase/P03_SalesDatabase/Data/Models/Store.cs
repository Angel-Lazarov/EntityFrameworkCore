namespace P03_SalesDatabase.Data.Models
{
    public class Store
    {
        public Store()
        {
            Sales = new HashSet<Sale>();
        }
        public int StoreId { get; set; }

        //[MaxLength(20)]
        public string Name { get; set; } = null!;

        public ICollection<Sale> Sales { get; set; }

    }
}
