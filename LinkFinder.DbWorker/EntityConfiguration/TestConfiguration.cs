using LinkFinder.DbWorker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace LinkFinder.DbWorker.EntityConfiguration
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.Property(p => p.TimeCreated)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
