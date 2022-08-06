using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Footballers.Data.Models;
using Footballers.Data.Models.Enums;
using Footballers.DataProcessor.ImportDto;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportCoachDto[]),
                new XmlRootAttribute("Coaches"));

            ImportCoachDto[] coachesDtos = (ImportCoachDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Coach> coaches = new List<Coach>();
            foreach (var coachDto in coachesDtos)
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<Footballer> footballers = new List<Footballer>();
                foreach (var footballerDto in coachDto.Footballers)
                {
                    if (!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime contractStartDate;
                    bool isValidContractStartDate =
                        DateTime.TryParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out contractStartDate);

                    DateTime contractEndDate;
                    bool isValidContractEndDate = DateTime.TryParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out contractEndDate);

                    BestSkillType bestSkillType;
                    bool isValidBestSkillType =
                        Enum.TryParse<BestSkillType>(footballerDto.BestSkillType, out bestSkillType);

                    PositionType positionType;
                    bool isValidPositionType =
                        Enum.TryParse<PositionType>(footballerDto.PositionType, out positionType);

                    if (!isValidContractStartDate ||
                        !isValidContractEndDate ||
                        !isValidBestSkillType || 
                        !isValidPositionType || 
                        contractEndDate < contractStartDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer f = new Footballer()
                    {
                        Name = footballerDto.Name,
                        BestSkillType = bestSkillType,
                        PositionType = positionType,
                        ContractStartDate = contractStartDate,
                        ContractEndDate = contractEndDate
                    };

                    footballers.Add(f);
                }

                Coach c = new Coach
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                    Footballers = footballers
                };

                coaches.Add(c);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, c.Name, c.Footballers.Count));
            }

            context.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTeamDto[] teamsDtos = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            List<Team> teams = new List<Team>();
            foreach (var teamDto in teamsDtos)
            {
                if (!IsValid(teamDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<TeamFootballer> teamFootballers = new List<TeamFootballer>();
                foreach (var footballerId in teamDto.Footballers.Distinct())
                {
                    if (context.Footballers.Find(footballerId) == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    TeamFootballer teamFootballer = new TeamFootballer
                    {
                        FootballerId = footballerId,
                    };

                    teamFootballers.Add(teamFootballer);
                }

                Team t = new Team
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies,
                    TeamsFootballers = teamFootballers
                };

                teams.Add(t);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, t.Name, t.TeamsFootballers.Count));
            }

            context.Teams.AddRange(teams);
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
