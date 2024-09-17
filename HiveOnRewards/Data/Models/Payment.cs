namespace HiveOnRewards.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public string Amount { get; set; }

        public string Timestamp { get; set; }

        public string Tx { get; set; }
    }
}