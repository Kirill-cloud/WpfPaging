using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WpfPaging.Models;

namespace WpfPaging.Services
{
    public class BankContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<ScoringSystemItem> ScoringSystems { get; set; }
        public DbSet<CreditPlan> CreditPlans { get; set; }
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bank;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
