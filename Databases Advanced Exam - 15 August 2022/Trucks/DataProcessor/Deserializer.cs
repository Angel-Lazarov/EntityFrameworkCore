namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;
    using Trucks.Utilities;

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

            var deserializedDtos = helper.Deserialize<ImportDespetcherDto[]>(xmlString, "Despatchers");
            List<Despatcher> despatchersToImport = new List<Despatcher>();

            foreach (var dto in deserializedDtos)
            {
                if (!IsValid(dto) || string.IsNullOrEmpty(dto.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher newDespatcher = new Despatcher()
                {
                    Name = dto.Name,
                    Position = dto.Position,
                };

                foreach (var td in dto.Trucks)
                {
                    if (!IsValid(td))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Truck newTruck = new Truck()
                    {
                        RegistrationNumber = td.RegistrationNumber,
                        VinNumber = td.VinNumber,
                        TankCapacity = td.TankCapacity,
                        CargoCapacity = td.CargoCapacity,
                        CategoryType = (CategoryType)td.CategoryType,
                        MakeType = (MakeType)td.MakeType
                    };

                    newDespatcher.Trucks.Add(newTruck);
                }

                despatchersToImport.Add(newDespatcher);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, newDespatcher.Name, newDespatcher.Trucks.Count));
            }

            context.Despatchers.AddRange(despatchersToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new();

            var deserializedDtos = JsonConvert.DeserializeObject<ImportClientDto[]>(jsonString);

            var validTruckIds = context.Trucks.Select(t => t.Id).ToList();

            List<Client> clientsToImport = new List<Client>();

            foreach (var dto in deserializedDtos)
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

                foreach (var truckId in dto.Trucks)
                {
                    if (!IsValid(truckId) || validTruckIds.Contains(truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck newClientTruck = new ClientTruck()
                    {
                        TruckId = truckId,
                        Client = newClient,
                    };

                    newClient.ClientsTrucks.Add(newClientTruck);
                }

                clientsToImport.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, newClient.Name, newClient.ClientsTrucks.Count));
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