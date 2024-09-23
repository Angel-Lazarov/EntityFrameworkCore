using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Data.Models;

public class TourPackage
{
    public int Id { get; set; }

    [MaxLength(40)]
    public string PackageName { get; set; } = null!;

    [MaxLength(200)]
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; } = new List<TourPackageGuide>();
}