using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Student_Assistant.Models
{

    public class CalendarContext : DbContext
    {
        public DbSet<LoginU> LoginU { get; set; }
        public DbSet<Roles>  Roles { get; set; }
        public DbSet<Schedule>  Schedules { get; set; }
        public DbSet<Subject>  Subjects { get; set; }
        public DbSet<Timetable>  Timetables { get; set; }
        public DbSet<User>  Users { get; set; }
        public DbSet<Work>  Works { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Filename=C:\Users\sipsl\Desktop\політех\програмкИ\sqlit\sqlit\DB\Calendar.sqlite");
        }
    }
}
