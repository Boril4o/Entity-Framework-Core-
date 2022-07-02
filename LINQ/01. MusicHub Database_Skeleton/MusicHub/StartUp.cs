using System.Linq;
using System.Text;
using MusicHub.Data.Models;

namespace MusicHub
{
    using System;

    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            int duration = int.Parse(Console.ReadLine());
            Console.WriteLine(ExportSongsAboveDuration(context, duration));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context
                .Producers
                .Find(producerId)
                .Albums
                .Select(a => new
                {
                    AlbumName = a.Name,
                    a.ReleaseDate,
                    ProducerName = a.Producer.Name,
                    a.Songs,
                    a.Price
                })
                .OrderByDescending(a => a.Price)
                .ToArray();



            StringBuilder sb = new StringBuilder();
            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}\n" +
                              $"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}\n" +
                              $"-ProducerName: {album.ProducerName}\n" +
                              $"-Songs:");

                var songs = album
                    .Songs
                    .Select(s => new
                    {
                        s.Name,
                        s.Price,
                        WriterName = s.Writer.Name
                    })
                    .OrderByDescending(s => s.Name)
                    .ThenBy(s => s.WriterName);

                int count = 0;
                foreach (var song in songs)
                {
                    sb.AppendLine($"---#{++count}\n" +
                                  $"---SongName: {song.Name}\n" +
                                  $"---Price: {song.Price:f2}\n" +
                                  $"---Writer: {song.WriterName}");
                }

                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }

            return sb.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context
                .Songs
                .Select(s => new
                {
                    s.Name,
                    PerformerFullName = s.SongPerformers
                        .Select(p => p.Performer.FirstName + " " + p.Performer.LastName)
                        .FirstOrDefault(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    s.Duration
                })
                .ToArray()
                .Where(s => s.Duration > TimeSpan.FromSeconds(duration))
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName)
                .ThenBy(s => s.PerformerFullName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{++count}\n" +
                              $"---SongName: {song.Name}\n" +
                              $"---Writer: {song.WriterName}\n" +
                              $"---Performer: {song.PerformerFullName}\n" +
                              $"---AlbumProducer: {song.AlbumProducer}\n" +
                              $"---Duration: {song.Duration.ToString("c")}");
            }

            return sb.ToString().Trim();
        }
    }
}
