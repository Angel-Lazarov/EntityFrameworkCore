using System.Globalization;
using System.Text;

namespace MusicHub
{
    using Data;
    using Initializer;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here

            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }



        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var sb = new StringBuilder();
            var albumInfo = context.Albums
                .Where(a => a.ProducerId.HasValue && a.ProducerId.Value == producerId)
                .ToArray()

                .OrderByDescending(a => a.Price)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                        .Select(s => new
                        {
                            SongName = s.Name,
                            SongPrice = s.Price.ToString("f2"),
                            WriterName = s.Writer.Name
                        })
                        .OrderByDescending(s => s.SongName)
                        .ThenBy(s => s.WriterName)
                        .ToArray(),

                    AlbumPrice = a.Price.ToString("f2")
                })
                .ToArray();


            foreach (var a in albumInfo)
            {
                int songNumber = 1;

                sb.AppendLine($"-AlbumName: {a.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {a.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {a.ProducerName}");
                sb.AppendLine($"-Songs:");

                foreach (var s in a.Songs)
                {
                    sb.AppendLine($"---#{songNumber}");
                    sb.AppendLine($"---SongName: {s.SongName}");
                    sb.AppendLine($"---Price: {s.SongPrice}");
                    sb.AppendLine($"---Writer: {s.WriterName}");
                    songNumber++;
                }

                sb.AppendLine($"-AlbumPrice: {a.AlbumPrice}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songInfo = context.Songs
                .AsEnumerable()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    WriterName = s.Writer.Name,
                    Performers = s.SongPerformers
                        .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                        .OrderBy(p => p)
                        .ToArray(),
                    AlbumProducerName = s.Album!.Producer!.Name,
                    Duration = s.Duration.ToString("c")
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ToArray();

            var sb = new StringBuilder();
            int songNumber = 1;

            foreach (var s in songInfo)
            {
                sb.AppendLine($"-Song #{songNumber}")
                    .AppendLine($"---SongName: {s.SongName}")
                    .AppendLine($"---Writer: {s.WriterName}");

                foreach (var performer in s.Performers)
                {
                    sb.AppendLine($"---Performer: {performer}");
                }

                sb.AppendLine($"---AlbumProducer: {s.AlbumProducerName}");
                sb.AppendLine($"---Duration: {s.Duration}");

                songNumber++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
