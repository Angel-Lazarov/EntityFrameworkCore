namespace MusicHub
{
    using Data;
    using Initializer;
    using System;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
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
                        songName = s.Name,
                        SongPrice = s.Price.ToString("F2"),
                        SongWriterName = s.Writer.Name
                    })
                    .OrderByDescending(s => s.songName)
                    .ThenBy(s => s.SongWriterName)
                    .ToArray()
                });

            var sb = new StringBuilder();

            foreach (var album in albumInfo)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}")
                    .AppendLine($"-ReleaseDate: {}")
            }


            return null;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
