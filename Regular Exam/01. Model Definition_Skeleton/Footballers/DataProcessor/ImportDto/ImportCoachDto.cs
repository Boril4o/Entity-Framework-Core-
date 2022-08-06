using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Footballers.Common;
using Footballers.Data.Models;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(ValidationConstans.COACH_NAME_MAX_LENGTH)]
        [MinLength(ValidationConstans.COACH_NAME_MIN_LENGTH)]
        public string Name { get; set; }

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; }

        [XmlArray("Footballers")]
        public FootballerDto[] Footballers { get; set; }
    }
}
