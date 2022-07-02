using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }

        [Key]
        public int UserId { get; set; }

        [StringLength(50)]
        [Required]
        public string Username { get; set; }

        [StringLength(300)]
        [Required]
        public string Password { get; set; }

        [StringLength(320)]
        [Required]
        public string Email { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
