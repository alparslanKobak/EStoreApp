using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P013EStore.Core.Entities;

namespace P013EStore.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        { // Özellikle string veri tiplerini sınırlayarak istediğimiz ayarları vermeye çalışırız.

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Image).HasMaxLength(50);
            
            builder.Property(x => x.ProductCode).HasMaxLength(50);

            // FluentAPI ile class lar arası ilişki kurma

            builder.HasOne(x=> x.Brand).WithMany(x=> x.Products).HasForeignKey(f=> f.BrandId); 

            // Brand class'ı ile Products class'ı arasında 1'e çok ilişki vardır.

            builder.HasOne(x=> x.Category).WithMany(x=> x.Products).HasForeignKey(f=> f.CategoryId);

            // Category class'ı ile Products class'ı arasında 1'e çok ilişki vardır.

           

        }
    }
}
