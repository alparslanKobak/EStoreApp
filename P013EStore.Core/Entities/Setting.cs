using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace P013EStore.Core.Entities
{
    public class Setting : IEntity
    {
        // Dinamik web uygulamalarında bu şekilde bir dinamik ayarlar menüsü iyi olabilir.
        public int Id { get; set; }

        [Display(Name = "Site Başlık")]
        public string? Title { get; set; }

        [Display(Name = "Site Açıklama"), DataType(DataType.MultilineText)]
        public string? Description { get; set; }


        public string? Email { get; set; }

        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Display(Name = "Mail Sunucusu")]
        public string? MailServer { get; set; }

        public int Port { get; set; }

        [Display(Name = "Mail Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Display(Name = "Mail Şifre")]
        public string? Password { get; set; }

        [Display(Name = "Site İkonu ")]
        public string? Favicon { get; set; }

        [Display(Name = "Site Logosu")]
        public string? Logo { get; set; }

        [Display(Name = "Firma Adresi"), DataType(DataType.MultilineText)]
        public string? Address { get; set; }

        [Display(Name = "Firma Harita Kodu"), DataType(DataType.MultilineText)]
        public string? MapCode { get; set; }
    }
}
