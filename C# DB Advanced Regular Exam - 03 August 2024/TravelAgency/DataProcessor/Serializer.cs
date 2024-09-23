using Newtonsoft.Json;
using System.Globalization;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            XmlHelper helper = new XmlHelper();

            var guides = context.Guides
                .Where(g => g.Language == Language.Spanish)
                .Select(g => new ExportGuideDto()
                {
                    FullName = g.FullName,
                    TourPackages = g.TourPackagesGuides
                        .OrderByDescending(g => g.TourPackage.Price)
                        .ThenBy(tp => tp.TourPackage.PackageName)
                        .Select(tp => new ExportTourPackageDto()
                        {
                            Name = tp.TourPackage.PackageName,
                            Description = tp.TourPackage.Description,
                            Price = tp.TourPackage.Price
                        })

                        .ToArray()

                })
                .OrderByDescending(g => g.TourPackages.Length)
                .ThenBy(g => g.FullName)
                .ToArray();

            return helper.Serialize<ExportGuideDto[]>(guides, "Guides");
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {

            var customers = context.Customers
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .Select(c => new
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                        .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                        .OrderBy(b => b.BookingDate)
                        .Select(b => new
                        {
                            TourPackageName = b.TourPackage.PackageName,
                            Date = b.BookingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        })

                })
                .OrderByDescending(c => c.Bookings.Count())
                .ThenBy(c => c.FullName)
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);

        }
    }
}
