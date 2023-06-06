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
        [Route("tum-urunlerimiz")] // adres çubuğundan tum-urunlerimiz yazınca bu action çalışsın
        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> model = await _serviceProduct.GetAllAsync(p=> p.Isactive);

            return View(model);
        }
        
        public async Task<IActionResult> Search(string? q) // Adres çubuğunda query string ile 
        {

            if (q== null)
            {
                return View();
            }
            IEnumerable<Product> model = await _serviceProduct.GetProductsByIncludeAsync(p=> p.Isactive && p.Name.Contains(q) || p.Name.Contains(q) || p.Description.Contains(q) || p.Brand.Name.Contains(q) || p.Category.Name.Contains(q));

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

            model.RelatedProducts = await _serviceProduct.GetAllAsync(P=> P.CategoryId == product.CategoryId && P.Id != id);

           

            

            if (model == null)
            {
                return BadRequest();
            }
            return View(model);
        }
    }
}
