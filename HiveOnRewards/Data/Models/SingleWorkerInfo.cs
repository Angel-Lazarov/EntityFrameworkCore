using System.Numerics;

namespace HiveOnRewards.Data.Models
{
    public class SingleWorkerInfo
    {
        public int Id { get; set; }

        public BigInteger LastBeat { get; set; }

        public int Hr { get; set; }
        public bool Offline { get; set; }

        public int Hr2 { get; set; }
        public int Rhr { get; set; }

        public int SharesValid { get; set; }

        public int SharesInvalid { get; set; }
        public int SharesStale { get; set; }
    }
}