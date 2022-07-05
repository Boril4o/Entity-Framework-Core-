using System.Collections.Generic;
using System.Linq;
using FastFood.Services.DTO.Employee;
using FastFood.Services.DTO.Position;
using FastFood.Services.Interfaces;

namespace FastFood.Core.Controllers
{
    using System;
    using AutoMapper;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Employees;

    public class EmployeesController : Controller
    {
        private readonly IPositionService positionService;
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;

        public EmployeesController(IPositionService positionService, IEmployeeService employeeService, IMapper mapper)
        {
            this.positionService = positionService;
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        public IActionResult Register()
        {
            ICollection<EmployeeRegisterPositionsAvailable> positionsDto = this.positionService.GetPositionsAvailable();

            List<RegisterEmployeeViewModel> regViewModel =
                this.mapper
                    .Map<ICollection<EmployeeRegisterPositionsAvailable>, ICollection<RegisterEmployeeViewModel>>(
                        positionsDto)
                    .ToList();

            return this.View(regViewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterEmployeeInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Register");
            }

            RegisterEmployeeDTO registerEmployee =
                this.mapper.Map<RegisterEmployeeDTO>(model);

            employeeService.Register(registerEmployee);

            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            ICollection<ListAllEmployeesDTO> listAllEmployeesDtos = this.employeeService.All();

            List<EmployeesAllViewModel> allViewModels = this.mapper
                .Map<ICollection<ListAllEmployeesDTO>, ICollection<EmployeesAllViewModel>>(listAllEmployeesDtos)
                .ToList();

            return this.View(allViewModels);
        }
    }
}
