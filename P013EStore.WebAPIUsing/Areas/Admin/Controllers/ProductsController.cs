using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Utils;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        private readonly string _apiAdres = "https://localhost:7011/api/Products";

        private readonly string _apiAdresKategori = "https://localhost:7011/api/Categories";

        private readonly string _apiAdresMarka = "https://localhost:7011/api/Brands";

        // GET: ProductsController
        public async Task<ActionResult> Index()
        {


            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdresKategori);
            ViewBag.CategoryId = new SelectList(data, "Id", "Name");

            var data1 = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdresMarka);
            ViewBag.BrandId = new SelectList(data1, "Id", "Name");

            var model = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres); // _httpClient nesnesi içindeki GetFromJsonAsync metodu kendisine verdiğimiz _apiAdres'deki URL'e get isteği gönderir ve oradan gelen json formatındaki app user listesini List<AppUser> nesnesine dönüştürür.

            return View(model);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public async Task<ActionResult> Create()
        {
            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdresKategori); 
            ViewBag.CategoryId = new SelectList(data, "Id", "Name");

            var data1 = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdresMarka); 
            ViewBag.BrandId = new SelectList(data1, "Id", "Name");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product collection, IFormFile? Image)
        {
            try
            {
                if(Image != null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                }
                var response = await _httpClient.PostAsJsonAsync(_apiAdres, collection);  // Veriyi Json'a çevirip verilen adrese yolladık.

                if (response.IsSuccessStatusCode) // Api'den başarılı istek kodu geldiyse...
                {
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Hata Oluştu" + e.Message);
            }

            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdresKategori);
            ViewBag.CategoryId = new SelectList(data, "Id", "Name");

            var data1 = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdresMarka);
            ViewBag.BrandId = new SelectList(data1, "Id", "Name");

            return View(collection);
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdresKategori);
            ViewBag.CategoryId = new SelectList(data, "Id", "Name");

            var data1 = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdresMarka);
            ViewBag.BrandId = new SelectList(data1, "Id", "Name");
            var model = await _httpClient.GetFromJsonAsync<Product>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,Product collection, IFormFile? Image)
        {
            try
            {
                if (Image != null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                }
                var response = await _httpClient.PutAsJsonAsync(_apiAdres, collection);  // Veriyi Json'a çevirip verilen adrese yolladık.

                if (response.IsSuccessStatusCode) // Api'den başarılı istek kodu geldiyse...
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Hata Oluştu" + e.Message);
            }
            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdresKategori);
            ViewBag.CategoryId = new SelectList(data, "Id", "Name");

            var data1 = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdresMarka);
            ViewBag.BrandId = new SelectList(data1, "Id", "Name");
            return View(collection);
        }

        // GET: ProductsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Product>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Product collection)
        {
            try
            {
                if (collection.Image is not null)
                {
                    FileHelper.FileRemover(collection.Image);

                }

                await _httpClient.DeleteAsync(_apiAdres + "/" + id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
