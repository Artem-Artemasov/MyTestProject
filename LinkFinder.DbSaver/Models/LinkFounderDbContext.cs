using LinkFinder.DbSaver.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace LinkFinder.DbSaver
{
    public class LinkFounderDbContext : DbContext, IEfRepositoryDbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<Result> Results { get; set; }

        public LinkFounderDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkFounderDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
