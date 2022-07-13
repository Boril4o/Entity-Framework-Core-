using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Constraints;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheaterDto
    {
        [Required]
        [MaxLength(GlobalConstraints.THEATRE_NAME_MAX_LENGTH)]
        [MinLength(GlobalConstraints.THEATRE_NAME_MIN_LENGTH)]
        public string Name { get; set; }

        [Range(typeof(sbyte), GlobalConstraints.THEATRE_NUMBER_OF_HALLS_MIN,
            GlobalConstraints.THEATRE_NUMBER_OF_HALLS_MAX)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MaxLength(GlobalConstraints.THEATRE_DIRECTOR_MAX_LENGTH)]
        [MinLength(GlobalConstraints.THEATRE_DIRECTOR_MIN_LENGTH)]
        public string Director { get; set; }

        public List<ImportTicketDto> Tickets { get; set; }
    }
}
