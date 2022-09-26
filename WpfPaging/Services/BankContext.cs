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
        public DbContextOptions _options { get; }

        public BankContext()
        {

        }

        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {
            _options = options;
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (_options == null)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Bank;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
    }
}
