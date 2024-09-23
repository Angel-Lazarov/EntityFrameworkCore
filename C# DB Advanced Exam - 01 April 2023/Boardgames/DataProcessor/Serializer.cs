using Boardgames.DataProcessor.ExportDto;
using Boardgames.Utilities;
using Newtonsoft.Json;
using System.Text;

namespace Boardgames.DataProcessor
{
    using Data;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            XmlHelper helper = new XmlHelper();
            string xmlRoot = "Creators";
            StringBuilder sb = new StringBuilder();

            var creators = context.Creators
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorDto()
                {
                    BoardgamesCount = c.Boardgames.Count,
                    CreatorName = c.FirstName + " " + c.LastName,
                    Boardgames = c.Boardgames
                        .Select(bg => new ExportBoardgameDto()
                        {
                            BoardgameName = bg.Name,
                            BoardgameYearPublished = bg.YearPublished
                        })
                        .OrderBy(bg => bg.BoardgameName)
                        .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            var result = helper.Serialize(creators, xmlRoot);


            return result;
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(bg => bg.Boardgame.YearPublished >= year)
                            && s.BoardgamesSellers.Any(bs => bs.Boardgame.Rating <= rating))
                .Select(s => new
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                        .Where(bs => bs.Boardgame.YearPublished >= year
                                     && bs.Boardgame.Rating <= rating)
                        .Select(bs => new
                        {
                            Name = bs.Boardgame.Name,
                            Rating = bs.Boardgame.Rating,
                            Mechanics = bs.Boardgame.Mechanics,
                            Category = bs.Boardgame.CategoryType.ToString()
                        })
                        .OrderByDescending(bs => bs.Rating)
                        .ThenBy(bs => bs.Name)
                        .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Length)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            var sellersToReturn = JsonConvert.SerializeObject(sellers, Formatting.Indented);

            return sellersToReturn;
        }
    }
}