using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Footballers.Common;

namespace Footballers.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstans.TEAM_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstans.TEAM_NATIOANALITY_MAX_LENGTH)]
        public string Nationality { get; set; }

        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}
