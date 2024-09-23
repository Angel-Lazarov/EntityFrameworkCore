using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using Boardgames.DataProcessor.ImportDto;
using Boardgames.Utilities;
using Newtonsoft.Json;
using System.Text;

namespace Boardgames.DataProcessor
{
    using Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {//44
            StringBuilder sb = new();
            XmlHelper helper = new XmlHelper();
            string xmlRoot = "Creators";

            ImportCreatorDto[] deserializedDtos = helper.Deserialize<ImportCreatorDto[]>(xmlString, xmlRoot);

            ICollection<Creator> creatorsToImport = new List<Creator>();

            foreach (ImportCreatorDto creatorDto in deserializedDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                ICollection<Boardgame> boardgamesToImport = new List<Boardgame>();

                foreach (var boardgame in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgame))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame newBoardgame = new Boardgame()
                    {
                        Name = boardgame.Name,
                        Rating = boardgame.Rating,
                        YearPublished = boardgame.YearPublished,
                        CategoryType = (CategoryType)boardgame.CategoryType,
                        Mechanics = boardgame.Mechanics
                    };

                    boardgamesToImport.Add(newBoardgame);
                }
                Creator newCreator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName,
                    Boardgames = boardgamesToImport
                };

                creatorsToImport.Add(newCreator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, newCreator.FirstName, newCreator.LastName,
                    newCreator.Boardgames.Count));
            }

            context.Creators.AddRange(creatorsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            StringBuilder sb = new();

            ImportSellerDto[] deserializedSellers = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString)!;

            ICollection<Seller> sellersToImport = new List<Seller>();

            foreach (ImportSellerDto sellerDto in deserializedSellers)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                // check website !!!!

                Seller newSeller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website
                };

                ICollection<BoardgameSeller> boardgamesSellersToImport = new List<BoardgameSeller>();

                foreach (int boardgameId in sellerDto.Boardgames.Distinct())
                {
                    if (!context.Boardgames.Any(b => b.Id == boardgameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    BoardgameSeller newBoardgameSeller = new BoardgameSeller()
                    {
                        BoardgameId = boardgameId,
                        Seller = newSeller
                    };

                    boardgamesSellersToImport.Add(newBoardgameSeller);
                }

                newSeller.BoardgamesSellers = boardgamesSellersToImport;

                sellersToImport.Add(newSeller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, newSeller.Name,
                    newSeller.BoardgamesSellers.Count));
            }

            context.Sellers.AddRange(sellersToImport);
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
