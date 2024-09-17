using System.Numerics;

namespace HiveOnRewards.Data.Models
{
    public class Info
    {
        public int Id { get; set; }

        public int Hnumreward { get; set; }

        public int Hreward { get; set; }

        public int ApiVersion { get; set; }

        public Config Config { get; set; }

        public string CurrentHashrate { get; set; }

        public double CurrentLuck { get; set; }

        public int Hashrate { get; set; }

        public int PageSize { get; set; }

        public Payment[] Payments { get; set; }

        public int PaymentsTotal { get; set; }

        public Reward[] Rewards { get; set; }

        public int RoundShares { get; set; }

        public int SharesInvalid { get; set; }
        public int SharesStale { get; set; }
        public int SharesValid { get; set; }

        public Stat Stats { get; set; }

        public Sumreward[] Sumrewards { get; set; }
        public BigInteger UpdatedAt { get; set; }

        public Worker[] Workers { get; set; }


    }
}
