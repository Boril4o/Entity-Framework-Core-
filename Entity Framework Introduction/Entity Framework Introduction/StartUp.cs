using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        private static void Main(string[] args)
        {
            var context = new SoftUniContext();

            Console.WriteLine(RemoveTown(context));
        }

        //Problem 1
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allEmployees = context
                .Employees
                .Select(e => new
                {
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .ToArray();

            foreach (var e in allEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 2
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.Salary > 50_000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 3
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Department = e.Department.Name,
                    e.Salary
                })
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department} - ${e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 4
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Add(address);

            Employee employeeNakov = context
                .Employees
                .First(e => e.LastName == "Nakov");

            employeeNakov.Address = address;

            context.SaveChanges();

            var employeesAddresses = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => new
                {
                    e.Address.AddressText
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employeesAddresses)
            {
                sb.AppendLine(employee.AddressText);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 5
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesProjects
                    .Any(ep => ep.Project.StartDate.Year >= 2001 &&
                               ep.Project.StartDate.Year <= 2003))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects.Select(ep => new
                    {
                        ProjectName = ep.Project.Name,
                        ProjectStartDate = ep.Project.StartDate,
                        ProjectEndDate = ep.Project.EndDate
                    })
                })
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    var startDate = project.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt");
                    var endDate = project.ProjectEndDate.HasValue ? project.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";

                    sb.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //Problem 6
        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context
                .Addresses
                .Select(a => new
                {
                    employeesCount = a.Employees.Count,
                    a.AddressText,
                    TownName = a.Town.Name
                })
                .OrderByDescending(a => a.employeesCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.employeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 7
        public static string GetEmployee147(SoftUniContext context)
        {
            const int id = 147;

            var employee147 = context
                .Employees
                .Find(id);

            var employeeProjects = context
                .EmployeesProjects
                .Where(p => p.Employee.EmployeeId == id)
                .OrderBy(p => p.Project.Name)
                .Select(p => new
                {
                    ProjectName = p.Project.Name
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            foreach (var project in employeeProjects)
            {
                sb.AppendLine(project.ProjectName);
            }

            return sb.ToString().TrimEnd();
        }

        //problem 8
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context
                .Departments
                .Select(d => new
                {
                    EmployeesCount = d.Employees.Count,
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastNamed = d.Manager.LastName,
                    Employees = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })

                })
                .Where(d => d.EmployeesCount > 5)
                .OrderBy(d => d.EmployeesCount)
                .ThenBy(d => d.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var department in departments)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerFirstName} {department.ManagerLastNamed}");

                foreach (var employee in department.Employees
                             .OrderBy(e => e.FirstName)
                             .ThenBy(e => e.LastName))
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 9
        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context
                .Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .ToArray();

            var projectsSorted = from a in projects
                orderby a.Name
                select a;

            StringBuilder sb = new StringBuilder();
            foreach (var project in projectsSorted)
            {
                sb.AppendLine(project.Name);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 10
        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" ||
                            e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees
                         .OrderBy(e => e.FirstName)
                         .ThenBy(e => e.LastName))
            {
                employee.Salary *= 1.12M;
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        //Problem 11
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                if (employee.FirstName.StartsWith("sa")) continue;

                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 12
        public static string DeleteProjectById(SoftUniContext context)
        {
            const int id = 2;

            Project project = context.Projects.Find(id);

            var employeesProjects = context.EmployeesProjects.Where(x => x.ProjectId == id).ToArray();
            foreach (var item in employeesProjects)
            {
                context.EmployeesProjects.Remove(item); 
                context.SaveChanges();
            }

            context.Projects.Remove(project);

            context.SaveChanges();

            Project[] projects = context.Projects.Take(10).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 13
        public static string RemoveTown(SoftUniContext context)
        {
            var employees = context
                .Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToArray();

            foreach (var employee in employees)
            {
                employee.Address = null;
                context.SaveChanges();
            }

            var addressesToDelete = context
                .Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToArray();

            foreach (var address in addressesToDelete)
            {
                context.Addresses.Remove(address);
            }

            context.SaveChanges();

            var townsToDelete = context
                .Towns
                .Where(t => t.Name == "Seattle")
                .ToArray();

            foreach (var town in townsToDelete)
            {
                context.Towns.Remove(town);
            }

            context.SaveChanges();

            return $"{addressesToDelete.Length} addresses in Seattle were deleted";
        }
    }

}


