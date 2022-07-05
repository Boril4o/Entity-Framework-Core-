using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Services.DTO.Employee;
using FastFood.Services.Interfaces;

namespace FastFood.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper mapper;

        private readonly FastFoodContext context;

        public EmployeeService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public void Register(RegisterEmployeeDTO dto)
        {
            Employee employee = this.mapper.Map<Employee>(dto);

            context.Employees.Add(employee);
            context.SaveChanges();
        }

        public ICollection<ListAllEmployeesDTO> All()
            => this.context
                .Employees
                .ProjectTo<ListAllEmployeesDTO>(this.mapper.ConfigurationProvider)
                .ToList();
    }
}
