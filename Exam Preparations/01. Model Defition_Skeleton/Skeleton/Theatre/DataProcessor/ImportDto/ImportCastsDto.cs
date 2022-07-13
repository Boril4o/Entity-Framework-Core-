using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using Theatre.Constraints;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class ImportCastsDto
    {
        [XmlElement("FullName")]
        [Required]
        [MaxLength(GlobalConstraints.CAST_FULL_NAME_MAX_LENGTH)]
        [MinLength(GlobalConstraints.CAST_FULL_NAME_MIN_LENGTH)]
        public string FullName { get; set; }

        [XmlElement("IsMainCharacter")]
        public bool IsMainCharacter { get; set; }

        [XmlElement("PhoneNumber")]
        [MaxLength(GlobalConstraints.CAST_PHONE_NUMBER_MAX_LENGTH)]
        [MinLength(GlobalConstraints.CAST_PHONE_NUMBER_MIN_LENGTH)]
        [Required]
        [RegularExpression(@"\+44-[\d]{2}-[\d]{3}-[\d]{4}")]
        public string PhoneNumber { get; set; }

        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }

    //<Cast>

    //    <FullName>Van Tyson</FullName>

    //    <IsMainCharacter>false</IsMainCharacter>

    //    <PhoneNumber>+44-35-745-2774</PhoneNumber>

    //    <PlayId>26</PlayId> </Cast>
}
