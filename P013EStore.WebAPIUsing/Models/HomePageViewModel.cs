using P013EStore.Core.Entities;

namespace P013EStore.WebAPIUsing.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<Slider>? Sliders { get; set; }
       
        public IEnumerable<Product>? Products { get; set; }

       
    }
}
