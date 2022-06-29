using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniORM.App.Data.Entities
{
    public class EmployeesProject
    {
        [ForeignKey(nameof(Project))]
        [Key]
        [Required]
        public int ProjectId { get; set; }

        [ForeignKey(nameof(Employee))]
        [Key]
        [Required]
        public int EmployeeId { get; set; }

        public ICollection<Employee> Employee { get; set; }

        public ICollection<Project> Project { get; set; }
    }
}
