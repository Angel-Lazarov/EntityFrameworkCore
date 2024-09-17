namespace HiveOnRewards.Data.Models
{
    public class Reward
    {
        public int Id { get; set; }

        public string Blockheight { get; set; }

        public string Timestamp { get; set; }

        public int RewardAmount { get; set; }

        public double Percent { get; set; }

        public bool Immature { get; set; }
        public bool Orphan { get; set; }
        public bool Uncle { get; set; }
    }
}