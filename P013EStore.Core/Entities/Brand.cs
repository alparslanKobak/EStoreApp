using System.ComponentModel.DataAnnotations;

namespace P013EStore.Core.Entities
{
    public class Brand : IEntity // Public yapmayı unutma
    {
        public int Id { get; set; }

        [Display(Name ="Marka Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        public string? Logo { get; set; }

        [Display(Name = "Durum")]
        public bool IsActive { get; set; }

        [Display(Name = "Eklenme Tarihi"), ScaffoldColumn(false)]

        public DateTime? CreateDate { get; set; } = DateTime.Now;

        public List<Product>? Products { get; set; }

    }
}
