using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IService<Brand> _serviceBrands;
        private readonly IService<Product> _serviceProduct;

        public BrandsController(IService<Brand> serviceBrands, IService<Product> serviceProduct)
        {
            _serviceBrands = serviceBrands;
            _serviceProduct = serviceProduct;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Brand brand = await _serviceBrands.FindAsync(id.Value);

            brand.Products = await _serviceProduct.GetAllAsync(p=> p.BrandId == id.Value);

            return View(brand);

            
        }
    }
}
