using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace P013EStore.Core.Entities
{
    public class News : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Resim")]
        public string? Image { get; set; }

        [Display(Name = "Durum")]
        public bool IsActive { get; set; }

        [Display(Name = "Ana Sayfada Göster")]
        public bool IsHome { get; set; }

        [Display(Name = "Üst Kategori")]
        public int ParentId { get; set; }

        [Display(Name = "Sıra No")]
        public int OrderNo { get; set; }

        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]

        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
