using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _serviceProduct;

        public ProductsController(IProductService serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> model = await _serviceProduct.GetAllAsync(p=> p.Isactive);

            return View(model);
        }

        public async Task<IActionResult> Detail(int? id)
        {

            if (id==null)
            {
                return NotFound();
            }

            ProductDetailViewModel model = new();

            Product product = await _serviceProduct.GetProductByIncludeAsync(id.Value);
            model.Product = product;

            model.RelatedProducts = await _serviceProduct.GetAllAsync(P=> P.CategoryId == product.CategoryId);

            model.Product = await _serviceProduct.GetProductByIncludeAsync(id.Value);

            

            if (model == null)
            {
                return BadRequest();
            }
            return View(model);
        }
    }
}
