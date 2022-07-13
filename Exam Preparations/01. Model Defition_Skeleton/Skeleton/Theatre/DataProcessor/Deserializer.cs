using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Theatre.Constraints;
using Theatre.Data;
using Theatre.Data.Models;
using Theatre.Data.Models.Enums;
using Theatre.DataProcessor.ImportDto;
using Theatre = Theatre.Data.Models.Theatre;

namespace Theatre.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportPlayDto[]),
                new XmlRootAttribute("Plays"));

            ImportPlayDto[] playsDtos =
                (ImportPlayDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Play> plays = new List<Play>(playsDtos.Length);
            foreach (var playDto in playsDtos)
            {
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan duration;
                bool isDurationValid = TimeSpan.TryParseExact(playDto.Duration, format: "c",
                    CultureInfo.InvariantCulture, out duration);

                Genre genre;
                bool isGenreValid = Enum.TryParse<Genre>(playDto.Genre, out genre);

                if (!isDurationValid ||
                    !isGenreValid ||
                    duration.Hours < GlobalConstraints.PLAY_DURATION_MIN_HOURS)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play p = new Play()
                {
                    Title = playDto.Title,
                    Duration = duration,
                    Rating = playDto.Rating,
                    Genre = genre,
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };

                plays.Add(p);

                sb.AppendLine(string.Format(SuccessfulImportPlay, playDto.Title, genre, playDto.Rating));
            }

            context.Plays.AddRange(plays);
           context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportCastsDto[]),
                new XmlRootAttribute("Casts"));

            ImportCastsDto[] castsDtos = 
                (ImportCastsDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Cast> casts = new List<Cast>();
            foreach (var castDto in castsDtos)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };

                if (!context.Plays.Select(x => x.Id).Contains(cast.PlayId))
                {
                    continue;
                }

                casts.Add(cast);

                string character = cast.IsMainCharacter ? "main" : "lesser";

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, character));
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTheaterDto[] theatersDtos = JsonConvert.DeserializeObject<ImportTheaterDto[]>(jsonString);

            List<Data.Models.Theatre> theatreList = new List<Data.Models.Theatre>();
            foreach (var theaterDto in theatersDtos)
            {
                if (!IsValid(theaterDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<ImportTicketDto> ticketList = new List<ImportTicketDto>();
                foreach (var ticketDto in theaterDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ticketList.Add(ticketDto);
                }

                Data.Models.Theatre t = new Data.Models.Theatre()
                {
                    Name = theaterDto.Name,
                    NumberOfHalls = theaterDto.NumberOfHalls,
                    Director = theaterDto.Director,
                    Tickets = ticketList.Select(t => new Ticket
                        {
                            Price = t.Price,
                            RowNumber = t.RowNumber,
                            PlayId = t.PlayId,
                        })
                        .ToList()
                };

                theatreList.Add(t);

                sb.AppendLine(string.Format(SuccessfulImportTheatre, t.Name, t.Tickets.Count));
            }

            context.Theatres.AddRange(theatreList);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
