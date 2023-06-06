using Microsoft.EntityFrameworkCore;
using P013EStore.Core.Entities;
using System.Reflection;

namespace P013EStore.Data
{
    public class DatabaseContext : DbContext
    {

        // Data ile ilgili Nuget Package Manager'i bu proje üzerine ekliyoruz. EStore içerisne Başlangıçtan itibaren eklemiyoruz. Yalnızca EStore.Data içerisine ;
        // - SQL SERVER
        // - TOOLS
        // paketlerini ekliyoruz.

        // Katmanlı mimaride bir proje katmanından başka bir katmana erişebilmek için Bulunduğumuz Data projesinin Dependencies kısmına sağ tıklayıp > Add Project References diyerek açılan pencereden Core projesine tik atıp ok diyerek pencereyi kapatmanız gerekiyor.

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // OnConfiguring metodu EntityFrameworkCore ile gelir ve veritabanı bağlantı ayarlarını yapmamızı sağlar. 



            optionsBuilder.UseSqlServer(@"Server =(localdb)\MSSQLLocalDB; Database = P013EStore; Trusted_Connection = True ");

            // Gerçek bir Hosting ile çalıştığımızda parametreler aşağıdaki gibidir : 

            //  optionsBuilder.UseSqlServer(@"Server =CanlıServerAdı; Database = CanlıHostingServerÜzerindekiDatabase; Username=CanlıVeritabanıKullanıcıAdı; Password=CanlıVeriTabanıŞifre ");



            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FluentAPI ile veritabanı tablolarımız oluşurken veri tiplerini db kurallarını burada tanımlayabiliriz.

            modelBuilder.Entity<AppUser>().Property(a=> a.Name).IsRequired().HasColumnType("nvarchar(50)").HasMaxLength(50); // FluentAPI ile AppUser class'ının Name Property'si için oluşacak veritabanı kolonu ayarlarını bu şekilde belirleyebiliyoruz.


            // nvarchar kullanarak türkçe karakter sorununu çözebiliriz.

            modelBuilder.Entity<AppUser>().Property(a => a.Surname).HasColumnType("nvarchar(50)").HasMaxLength(50);

            modelBuilder.Entity<AppUser>().Property(a => a.UserName).HasColumnType("nvarchar(50)").HasMaxLength(50);

            modelBuilder.Entity<AppUser>().Property(a => a.Password).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);

            // Is Required metodu, property tanımlanmasının zorunlu olup olmamasını belirler. Eğer metod var ise o property zorunludur.

            modelBuilder.Entity<AppUser>().Property(a => a.Email).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<AppUser>().Property(a => a.Phone).HasMaxLength(20);


            // FluentAPI HasData ile db oluştuktan sonra başlangıç kayıtları ekleme

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id =1,
                Email="info@P013EStore.com",
                Password="123",
                UserName="Admin",
                Isactive=true,
                IsAdmin=true,
                Name="Admin",
                UserGuid = Guid.NewGuid() // Kullanıcıya benzersiz bir id no oluştur.

            });

            // modelBuilder.ApplyConfiguration(new BrandConfigurations()); // marka için yaptığımız konfigürasyon ayarlarını çağırdık.

            // modelBuilder.ApplyConfiguration(new CategoryConfigurations());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Yukarıda tek tek yapacağımız uygulama konfigürasyonlarını kabul et metodunu tek seferde kabul ettirdik.

            // Uygulamadaki tüm configurations class'larını burada çalıştır.

            // Fluent Validation : Data annotiondaki hata mesajları vb. işlemlerini yönetebileceğimiz 3. parti paket.



            // Katmanlı mimaride MVCWebUI katmanından direkt data katmanına erişilmesi istenmez, arada bir iş katmanının tüm Db süreçlerini yönetmesi istenir. Bu yüzden;
            // - solutiona Service katmanı ekleyip MVC katmanından Service katmanına erişim vermemiz gerekir
            // - Service katmanı da Data katmanına erişir.
            // - Data katmanı da core katmanına erişir.
            // Böylece MVCUI > Service > Data > Core ile en üstten en alt katmana kadar ulaşılabilmiş olunur.


            // Data katmanını set as a startup project diyerek Nuget üzerinden Design paketini kulanmaya izin alık. Migration ve update database uygulamalarımızı bu şekilde gerçekleştirdik.

            base.OnModelCreating(modelBuilder);
        }

    }
}