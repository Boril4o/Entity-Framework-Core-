using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using SoftJail.Data.Models;
using SoftJail.Data.Models.Enums;
using SoftJail.DataProcessor.ImportDto;

namespace SoftJail.DataProcessor
{

    using Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var importDepartmentCells =
                JsonConvert.DeserializeObject<List<ImportDepartmentCellDto>>(jsonString);

            List<Department> departments = new List<Department>();
            foreach (var departmentCellDto in importDepartmentCells)
            {
                if (!IsValid(departmentCellDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Department department = new Department()
                {
                    Name = departmentCellDto.Name,
                };

                bool isValidDepth = true;
                foreach (var cellDto in departmentCellDto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        isValidDepth = false;
                        sb.AppendLine("Invalid Data");
                        break;
                    }

                    Cell c = new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    };

                    department.Cells.Add(c);
                }

                if (!isValidDepth)
                {
                    continue;
                }

                if (!department.Cells.Any())
                {
                    continue;
                }

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                departments.Add(department);
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();
            

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportPrisonersMailsDto[] prisonersDtos =
                JsonConvert.DeserializeObject<ImportPrisonersMailsDto[]>(jsonString);

            List<Prisoner> prisoners = new List<Prisoner>();
            List<Mail> mails = new List<Mail>();

            foreach (var prisonerDto in prisonersDtos)
            {
                if (!IsValid(prisonerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime incarcerationDateValid;
                bool isIncarcerationDateValid =
                    DateTime.TryParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out incarcerationDateValid);

                if (!isIncarcerationDateValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                DateTime? releaseDate = null;
                if (!string.IsNullOrEmpty(prisonerDto.ReleaseDate))
                {
                    DateTime releaseDateValue;
                    bool isReleaseDateValid = DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out releaseDateValue);

                    if (!isReleaseDateValid)
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    releaseDate = releaseDateValue;
                }

                Prisoner p = new Prisoner()
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = incarcerationDateValid,
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId,
                };

                bool isValidDepth = true;
                List<Mail> mailsDtos = new List<Mail>();
                foreach (var mailDto in prisonerDto.Mails)
                {
                    if (!IsValid(mailDto.Address))
                    {
                        isValidDepth = false;
                        break;
                    }

                    Mail mail = new Mail()
                    {
                        Address = mailDto.Address,
                        Description = mailDto.Description,
                        Sender = mailDto.Sender
                    };

                    mailsDtos.Add(mail);
                }

                if (!isValidDepth)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                p.Mails = mails;
                mails.AddRange(mailsDtos);
                prisoners.Add(p);

                sb.AppendLine($"Imported {p.FullName} {p.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportOfficerPrisonerDto[]),
                new XmlRootAttribute("Officers"));

            ImportOfficerPrisonerDto[] officerDtos = 
                (ImportOfficerPrisonerDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Officer> officers = new List<Officer>();
            List<OfficerPrisoner> officersPrisoners = new List<OfficerPrisoner>();

            foreach (var officerDto in officerDtos)
            {
                if (!IsValid(officerDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Position position;
                bool isPositionValid = Enum.TryParse<Position>(officerDto.Position, out position);

                Weapon weapon;
                bool isWeaponValid = Enum.TryParse<Weapon>(officerDto.Weapon, out weapon);

                if (!isWeaponValid || !isPositionValid)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = officerDto.Name,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = officerDto.DepartmentId,
                    Salary = officerDto.Money
                };

                List<OfficerPrisoner> op = new List<OfficerPrisoner>();
                foreach (var prisonerDto in officerDto.Prisoners)
                {
                    OfficerPrisoner officerPrisoner = new OfficerPrisoner()
                    {
                        OfficerId = officer.Id,
                        Officer = officer,
                        PrisonerId = prisonerDto.Id,
                    };

                    op.Add(officerPrisoner);
                }

                officer.OfficerPrisoners = op;
                officers.Add(officer);
                officersPrisoners.AddRange(op);

                sb.AppendLine($"Imported {officer.FullName} ({op.Count} prisoners)");
            }

            context.OfficersPrisoners.AddRange(officersPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}