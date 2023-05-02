using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P013EStore.Core.Entities;

namespace P013EStore.Data.Configurations
{
    internal class ContactConfigurations : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Surname).HasMaxLength(50);

            builder.Property(x => x.Phone).HasMaxLength(50);

            builder.Property(x => x.Message).IsRequired().HasMaxLength(500);
        }
    }
}
