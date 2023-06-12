using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;

namespace P013EStore.WebAPIUsing.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7011/api/Products";

        [Route("tum-urunlerimiz")]
        public async Task<IActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres); // _httpClient nesnesi içindeki GetFromJsonAsync metodu kendisine verdiğimiz _apiAdres'deki URL'e get isteği gönderir ve oradan gelen json formatındaki app user listesini List<AppUser> nesnesine dönüştürür.

            return View(model);
        }

        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            ProductDetailViewModel model = new();

            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres);

            var product = await _httpClient.GetFromJsonAsync<Product>(_apiAdres + "/" + id);

            model.Product = product;

            model.RelatedProducts = products.Where(p=> p.CategoryId == product.CategoryId && p.Id!= id).ToList();





            if (model == null)
            {
                return BadRequest();
            }
            return View(model);
        }

        public async Task<IActionResult> Search(string? q) // Adres çubuğunda query string ile 
        {

            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "/GetSearch/" + q);

            if (q == null)
            {
                return View();
            }
          
            return View(products);
        }
    }
}
