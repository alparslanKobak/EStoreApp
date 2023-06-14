using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P013EStore.Core.Entities;

namespace P013EStore.Data.Configurations
{
    internal class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasColumnType("nvarchar(50)").HasMaxLength(50);

            builder.Property(x => x.Logo).HasMaxLength(100);

            builder.Property(x => x.Favicon).HasMaxLength(50);

            builder.Property(x => x.Description).HasMaxLength(500);

            builder.Property(x => x.Phone).HasMaxLength(20);

            builder.Property(x => x.MailServer).HasMaxLength(100);

            builder.Property(x => x.UserName).HasMaxLength(50);

            builder.Property(x => x.Password).HasMaxLength(50);

            builder.Property(x => x.Address).HasMaxLength(200);

            builder.Property(x => x.MapCode).HasMaxLength(500);
        }
    }
}
