using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using Footballers.DataProcessor.ImportDto;
using Footballers.Utilities;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

namespace Footballers.DataProcessor
{
    using Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper helper = new XmlHelper();

            var deserializedCoaches = helper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            var coachesToImport = new List<Coach>();

            foreach (var coachDto in deserializedCoaches)
            {
                if (!IsValid(coachDto) || string.IsNullOrEmpty(coachDto.Nationality))
                // if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newCoach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                foreach (ImportFootballerDto footballer in coachDto.Footballers)
                {
                    if (!IsValid(footballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isContractStartDateValid
                        = DateTime.TryParseExact(footballer.ContractStartDate, "dd/MM/yyyy",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ContractStartDate);

                    bool isContractEndDateValid
                        = DateTime.TryParseExact(footballer.ContractEndDate, "dd/MM/yyyy",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ContractEndDate);

                    if (isContractStartDateValid == false || isContractEndDateValid == false)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    //if (DateTime.Compare(ContractStartDate, ContractEndDate) > 0)                   
                    if (ContractStartDate > ContractEndDate)      // !!!!!!!! =
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer newFootballer = new Footballer()
                    {
                        Name = footballer.Name,
                        ContractStartDate = ContractStartDate,
                        ContractEndDate = ContractEndDate,
                        BestSkillType = (BestSkillType)footballer.BestSkillType,
                        PositionType = (PositionType)footballer.PositionType
                    };
                    newCoach.Footballers.Add(newFootballer);
                }
                coachesToImport.Add(newCoach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, newCoach.Name, newCoach.Footballers.Count));
            }

            context.Coaches.AddRange(coachesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new();

            var deserializedDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            int[] validFootballerIds = context.Footballers.Select(f => f.Id).ToArray();

            ICollection<Team> teamsToImport = new List<Team>();


            foreach (ImportTeamDto dto in deserializedDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (dto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Team newTeam = new Team()
                {
                    Name = dto.Name,
                    Nationality = dto.Nationality,
                    Trophies = dto.Trophies,
                };


                foreach (int id in dto.FootballersIds.Distinct())
                {
                    if (!IsValid(id) || !validFootballerIds.Contains(id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer newTeamFootballer = new TeamFootballer()
                    {
                        FootballerId = id
                    };

                    newTeam.TeamsFootballers.Add(newTeamFootballer);
                }


                teamsToImport.Add(newTeam);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, newTeam.Name, newTeam.TeamsFootballers.Count));
            }

            context.Teams.AddRange(teamsToImport);
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
