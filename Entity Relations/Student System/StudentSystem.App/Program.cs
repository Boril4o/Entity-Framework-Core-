using System;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data;

namespace StudentSystem.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
           StudentSystemContext context = new StudentSystemContext();
           
           context.Database.Migrate();

           Console.WriteLine("Db created successfully!");
        }
    }
}
