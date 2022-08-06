using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Footballers.Common;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamDto
    {
        [Required]
        [MaxLength(ValidationConstans.TEAM_NAME_MAX_LENGTH)]
        [MinLength(ValidationConstans.TEAM_NAME_MIN_LENGTH)]
        [RegularExpression(ValidationConstans.TEAM_NAME_REGEX)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstans.TEAM_NATIOANALITY_MAX_LENGTH)]
        [MinLength(ValidationConstans.TEAM_NATIOANALITY_MIN_LENGTH)]
        public string Nationality { get; set; }

        [Range(typeof(int), ValidationConstans.TEAM_TROPHIES_MIN, ValidationConstans.TEAM_TROPHIES_MAX)]
        public int Trophies { get; set; }

        public int[] Footballers { get; set; }
    }
}
