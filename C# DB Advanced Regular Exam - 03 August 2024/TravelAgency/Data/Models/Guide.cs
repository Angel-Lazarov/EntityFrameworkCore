using System.ComponentModel.DataAnnotations;
using TravelAgency.Data.Models.Enums;

namespace TravelAgency.Data.Models;

public class Guide
{
    public int Id { get; set; }

    [MaxLength(60)]
    public string FullName { get; set; } = null!;

    public Language Language { get; set; }

    public virtual ICollection<TourPackageGuide> TourPackagesGuides { get; set; } = new List<TourPackageGuide>();

}