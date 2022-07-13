using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SoftJail.DataProcessor.ExportDto;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context
                .Prisoners
                .ToArray()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(po => new
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                        .OrderBy(x => x.OfficerName)
                        .ToArray(),

                    TotalOfficerSalary = 
                        p.PrisonerOfficers.Sum(x => x.Officer.Salary)
                            

                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id);

            return JsonConvert.SerializeObject(prisoners, formatting:Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            string[] prisonersNamesSplitted = prisonersNames.Split(",").ToArray();

            PrisonerExportDto[] prisonersExportDtos = context
                .Prisoners
                .ToArray()
                .Where(p => prisonersNamesSplitted.Contains(p.FullName))
                .Select(p => new PrisonerExportDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture),
                    EncryptedMessages = p.Mails.Select(m => new EncryptedMessagesDto
                    {
                        Description = string.Join("", m.Description.Reverse())
                    })
                        .ToArray()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(PrisonerExportDto[]), new XmlRootAttribute("Prisoners"));

            XmlSerializerNamespaces namespaces = 
                new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(sb))
            {
                xmlSerializer.Serialize(stringWriter, prisonersExportDtos, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}