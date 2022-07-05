using System;
using System.Collections.Generic;
using System.Text;
using FastFood.Services.DTO.Employee;

namespace FastFood.Services.Interfaces
{
    public interface IEmployeeService
    {
        void Register(RegisterEmployeeDTO dto);

        ICollection<ListAllEmployeesDTO> All();
    }
}
