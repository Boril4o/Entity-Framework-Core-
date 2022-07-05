using System;
using System.Collections.Generic;
using System.Text;

namespace FastFood.Services.DTO.Employee
{
    public class RegisterEmployeeDTO
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public int PositionId { get; set; }

        public string Address { get; set; }
    }
}
