using Microsoft.EntityFrameworkCore;
using System.Data;


namespace LinkFounder.DbSaver.Models
{
    public class LinkFounderDbContext : DbContext, IEfRepositoryDbContext
    {
        public LinkFounderDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
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
