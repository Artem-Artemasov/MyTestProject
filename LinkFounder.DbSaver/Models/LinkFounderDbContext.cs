using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFounder.DbSaver.Models
{
    public class LinkFounderDbContext : DbContext, IEfRepositoryDbContext
    {
        public LinkFounderDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkFounderDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Result> Results { get; set; }
    }
}
