using P013EStore.Core.Entities;

namespace P013EStore.MVCUI.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<Slider>? Sliders { get; set; }
       
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Brand>? Brands { get; set; }
        public IEnumerable<News>? News { get; set; }

       
    }
}
