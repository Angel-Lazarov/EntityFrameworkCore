using Newtonsoft.Json;
using System.Text;
using Trucks.Data.Models;
using Trucks.Data.Models.Enums;
using Trucks.DataProcessor.ImportDto;
using Trucks.Utilities;

namespace Trucks.DataProcessor
{
    using Data;
    using System.ComponentModel.DataAnnotations;


    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            XmlHelper helper = new XmlHelper();
            StringBuilder sb = new();

            var deserializedDtos = helper.Deserialize<ImportDespatcherDto[]>(xmlString, "Despatchers");

            ICollection<Despatcher> despatcherstoImport = new List<Despatcher>();

            foreach (ImportDespatcherDto dto in deserializedDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(dto.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher newDespatcher = new Despatcher()
                {
                    Name = dto.Name,
                    Position = dto.Position
                };

                foreach (var truck in dto.Trucks)
                {
                    if (!IsValid(truck))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck newTruck = new Truck()
                    {
                        RegistrationNumber = truck.RegistrationNumber,
                        VinNumber = truck.VinNumber,
                        TankCapacity = truck.TankCapacity,
                        CargoCapacity = truck.CargoCapacity,
                        CategoryType = (CategoryType)truck.CategoryType,
                        MakeType = (MakeType)truck.MakeType
                    };

                    newDespatcher.Trucks.Add(newTruck);
                }

                despatcherstoImport.Add(newDespatcher);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, newDespatcher.Name,
                    newDespatcher.Trucks.Count));
            }

            context.Despatchers.AddRange(despatcherstoImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new();
            var deserializedDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);
            int[] validTruckIds = context.Trucks.Select(t => t.Id).ToArray();
            var clientsToImport = new List<Client>();

            foreach (ImportClientDto dto in deserializedDtos)
            {
                if (!IsValid(dto) || dto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client newClient = new Client()
                {
                    Name = dto.Name,
                    Nationality = dto.Nationality,
                    Type = dto.Type
                };

                foreach (int dtoId in dto.TruckIds.Distinct())
                {
                    if (!IsValid(dtoId) || !validTruckIds.Contains(dtoId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck newClientTruck = new ClientTruck()
                    {
                        TruckId = dtoId,
                        Client = newClient
                    };

                    newClient.ClientsTrucks.Add(newClientTruck);
                }

                clientsToImport.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, newClient.Name, newClient.ClientsTrucks.Count()));
            }

            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}