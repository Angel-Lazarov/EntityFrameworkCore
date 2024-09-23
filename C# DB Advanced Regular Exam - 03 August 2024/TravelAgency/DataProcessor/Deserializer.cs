using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();
            StringBuilder sb = new();

            List<Customer> customerToImport = new List<Customer>();

            var deserializedDtos = helper.Deserialize<ImportCustomerDto[]>(xmlString, "Customers");

            foreach (ImportCustomerDto customerDto in deserializedDtos)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Customer newCustomer = new Customer()
                {
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber
                };

                if (customerToImport.Any(c =>
                        c.FullName == customerDto.FullName || c.Email == customerDto.Email ||
                        c.PhoneNumber == customerDto.PhoneNumber))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                customerToImport.Add(newCustomer);
                sb.AppendLine(string.Format(SuccessfullyImportedCustomer, newCustomer.FullName));
            }

            context.Customers.AddRange(customerToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new();

            var deserializedDtos = JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString);

            List<Booking> bookingsToImport = new List<Booking>();

            foreach (ImportBookingDto bookingDto in deserializedDtos)
            {
                if (!IsValid(bookingDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isValidDate = DateTime.TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate);

                if (!isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Booking newBooking = new Booking()
                {
                    BookingDate = bookingDate,
                    Customer = context.Customers.FirstOrDefault(c => c.FullName == bookingDto.CustomerName),
                    TourPackage = context.TourPackages.FirstOrDefault(tp => tp.PackageName == bookingDto.TourPackageName)
                };

                bookingsToImport.Add(newBooking);
                sb.AppendLine(string.Format(SuccessfullyImportedBooking, bookingDto.TourPackageName, bookingDate.ToString("yyyy-MM-dd")));
            }

            context.Bookings.AddRange(bookingsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
