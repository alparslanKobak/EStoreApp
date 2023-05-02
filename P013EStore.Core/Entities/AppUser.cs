using System.ComponentModel.DataAnnotations;

namespace P013EStore.Core.Entities
{
    public class AppUser : IEntity
    {
        public int Id { get; set; }

        [Display(Name ="Ad")]
        public string Name { get; set; }
        [Display(Name ="Soyad")]
        public string? Surname { get; set; }
        [Display(Name ="Email"),EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Display(Name = "Durum")]
        public bool Isactive { get; set; }

        [Display(Name = "Adminlik Yetkisi")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Eklenme Tarihi"),ScaffoldColumn(false)]

        public DateTime? CreateDate { get; set; } = DateTime.Now;


        public Guid? UserGuid { get; set; }
    }
}
