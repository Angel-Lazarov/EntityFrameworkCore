﻿using System.ComponentModel.DataAnnotations;

namespace Trucks.Data.Models
{
    public class Client
    {
        public int Id { get; set; }

        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        public string Type { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = new List<ClientTruck>();

    }
}
