using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [MaxLength(60)]
        public string FullName { get; set; } = null!;

        [MaxLength(50)]
        public string Email { get; set; } = null!;

        [MaxLength(13)]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
