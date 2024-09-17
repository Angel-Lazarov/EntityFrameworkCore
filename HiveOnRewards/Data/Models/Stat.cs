using System.Numerics;

namespace HiveOnRewards.Data.Models
{
    public class Stat
    {
        public int Id { get; set; }
        public BigInteger Balance { get; set; }

        public int BlocksFound { get; set; }

        public int Immature { get; set; }

        public BigInteger LastShare { get; set; }

        public BigInteger Paid { get; set; }
        public int Pending { get; set; }
    }
}