using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Theatre.Constraints;

namespace Theatre.Data.Models
{
    public class Cast
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstraints.CAST_FULL_NAME_MAX_LENGTH)]
        public string FullName { get; set; }

        public bool IsMainCharacter { get; set; }

        [Required]
        [MaxLength(GlobalConstraints.CAST_PHONE_NUMBER_MAX_LENGTH)]
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(Play))]
        public int PlayId { get; set; }
        [Required]
        public Play Play { get; set; }
    }
}
