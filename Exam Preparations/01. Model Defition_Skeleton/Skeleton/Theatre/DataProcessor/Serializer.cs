using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Theatre.DataProcessor.ExportDto;

namespace Theatre.DataProcessor
{
    using System;
    using Theatre.Data;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context
                .Theatres
                .ToArray()
                .Where(t => t.NumberOfHalls >= numbersOfHalls && t.Tickets.Count >= 20)
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                        .Where(x => x.RowNumber >= 1 && x.RowNumber <= 5)
                        .Sum(x => x.Price),
                    Tickets = t.Tickets
                        .Where(x => x.RowNumber >= 1 && x.RowNumber <= 5)
                        .Select(x => new
                        {
                            Price = x.Price,
                            RowNumber = x.RowNumber
                        })
                        .OrderByDescending(x => x.Price)

                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            ExportPlaysDto[] playsDtos = context
                .Plays
                .ToArray()
                .Where(p => p.Rating <= rating)
                .Select(p => new ExportPlaysDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                        .Where(a => a.IsMainCharacter)
                        .Select(a => new ExportActorDto()
                        {
                            FullName = a.FullName,
                            MainCharacter = $@"Plays main character in '{p.Title}'."
                        })
                        .OrderByDescending(a => a.FullName)
                        .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportPlaysDto[]), 
                new XmlRootAttribute("Plays"));

            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            serializer.Serialize(new StringWriter(sb), playsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
