using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Constraints;

namespace Theatre.Data.Models
{
    public class Theatre 
    {
        public Theatre()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(GlobalConstraints.THEATRE_NAME_MAX_LENGTH)]
        [Required]
        public string Name { get; set; }

        public sbyte NumberOfHalls { get; set; }

        [MaxLength(GlobalConstraints.THEATRE_DIRECTOR_MAX_LENGTH)]
        [Required]
        public string Director { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
