namespace HiveOnRewards.Data.Models
{
    public class Config
    {
        public int Id { get; set; }

        public string AllowedMaxPayout { get; set; }

        public string AllowedMinPayout { get; set; }
        public string DefaultMinPayout { get; set; }

        public string IpHint { get; set; }

        public string IpWorkerName { get; set; }

        public int MinPayout { get; set; }

        public string PaymentHubHint { get; set; }



    }
}
