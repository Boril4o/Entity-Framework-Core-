using System;
using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new FootballBettingContext();

            context.Database.Migrate();

            Console.WriteLine("Db created successfully!");
        }
    }
}
