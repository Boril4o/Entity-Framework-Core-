using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
            
        }

        public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
            : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentsCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(e =>
            {
                e.Property(s => s.PhoneNumber).IsUnicode(false);
            });

            modelBuilder.Entity<Resource>(e =>
            {
                e.Property(r => r.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Homework>(e =>
            {
                e.Property(h => h.Content).IsUnicode(false);
            });

            modelBuilder.Entity<StudentCourse>(e =>
            {
                e.HasKey("StudentId", "CourseId");
            });
        }
    }
}
