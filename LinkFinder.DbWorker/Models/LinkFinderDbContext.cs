using LinkFinder.DbWorker.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace LinkFinder.DbWorker
{
    public class LinkFinderDbContext : DbContext, IEfRepositoryDbContext
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<Result> Results { get; set; }

        public LinkFinderDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LinkFinderDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
