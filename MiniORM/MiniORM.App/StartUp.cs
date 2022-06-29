using System;
using System.Linq;
using MiniORM.App.Data;
using MiniORM.App.Data.Entities;

namespace MiniORM.App
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            string connectionString =
                @"Server=DESKTOP-JJFVSKV\SQLEXPRESS;Database=MiniORM;Integrated Security = true";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Boris",
                LastName = "Georgiev",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true
            });

            Employee employee = context.Employees.Last();

            context.SaveChanges();
        }
    }
}
