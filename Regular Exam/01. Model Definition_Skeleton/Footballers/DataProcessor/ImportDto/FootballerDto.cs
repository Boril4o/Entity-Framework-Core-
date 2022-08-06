using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Footballers.Common;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class FootballerDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(ValidationConstans.FOTBALLER_NAME_MAX_LENGTH)]
        [MinLength(ValidationConstans.FOTBALLER_NAME_MIN_LENGTH)]
        public string Name { get; set; }

        [Required]
        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; }

        [Required]
        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; }

        [Required]
        [XmlElement("BestSkillType")]
        public string BestSkillType { get; set; }

        [Required]
        [XmlElement("PositionType")]
        public string PositionType { get; set; }
    }
}
