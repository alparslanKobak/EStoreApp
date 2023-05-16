using P013EStore.Core.Entities;

namespace P013EStore.MVCUI.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<Slider>? Sliders { get; set; }
       
        public IEnumerable<Product>? Products { get; set; }

       
    }
}
