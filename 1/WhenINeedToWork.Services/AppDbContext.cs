using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WhenINeedToWork.Models;

namespace WhenINeedToWork.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }
        public DbSet<WhenINeedToWork.Models.Calendar> Calendars { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
