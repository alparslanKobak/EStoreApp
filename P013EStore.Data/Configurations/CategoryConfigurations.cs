using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P013EStore.Core.Entities;

namespace P013EStore.Data.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category> // Implement Interface demeyi unutma
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Image).HasMaxLength(100);


        }
    }
}
