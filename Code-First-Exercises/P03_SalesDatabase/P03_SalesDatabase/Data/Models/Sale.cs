﻿namespace P03_SalesDatabase.Data.Models
{
    public class Sale
    {
        public int SaleId { get; set; }

        public DateTime Date { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        public int StoreId { get; set; }

        public virtual Store Store { get; set; } = null!;
    }
}
