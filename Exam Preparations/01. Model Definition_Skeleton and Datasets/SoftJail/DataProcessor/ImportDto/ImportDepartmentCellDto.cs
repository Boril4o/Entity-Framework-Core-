using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentCellDto
    {
        [Required]
        [StringLength(maximumLength:25, MinimumLength = 3)]
        public string Name { get; set; }

        public DepartmentCellDto[] Cells { get; set; }
    }
}
