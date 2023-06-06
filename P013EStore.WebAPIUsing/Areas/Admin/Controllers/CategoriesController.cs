using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Utils;
using System.Reflection;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        private readonly string _apiAdres = "https://localhost:7011/api/Categories"; //Api adresini web api projesini çalıştırdığımızda adres çubuğundan veya herhangi bir controller'a istek atarak RequestURL kısmından veya web api projesinde Properties altındaki launchSettings.json 

        // GET: CategoriesController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres); // _httpClient nesnesi içindeki GetFromJsonAsync metodu kendisine verdiğimiz _apiAdres'deki URL'e get isteği gönderir ve oradan gelen json formatındaki app user listesini List<AppUser> nesnesine dönüştürür.

            return View(model);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriesController/Create
        public async Task<ActionResult> Create()
        {
            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres);

            ViewBag.ParentId = new SelectList(data, "Id", "Name");
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category collection, IFormFile? Image)
        {
            try
            {
                if (Image != null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);

                }
                var response = await _httpClient.PostAsJsonAsync(_apiAdres, collection);  // Veriyi Json'a çevirip verilen adrese yolladık.

                if (response.IsSuccessStatusCode) // Api'den başarılı istek kodu geldiyse...
                {
                    return RedirectToAction(nameof(Index));

                }
            }
            catch(Exception e)
            {
                ModelState.AddModelError("", "Hata Oluştu  : " + e.Message);
            }
            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres);

            ViewBag.ParentId = new SelectList(data, "Id", "Name");
            return View();
        }

        // GET: CategoriesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres);

            ViewBag.ParentId = new SelectList(data, "Id", "Name");
            var model = await _httpClient.GetFromJsonAsync<Category>(_apiAdres + "/" + id); // Json formatındaki slash işareti kullanımı dolayısıyla / kullandık.
            return View(model);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Category collection, IFormFile? Image, bool? resmiSil)
        {
            try
            {

                if (Image != null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                }


                if (resmiSil == true && resmiSil is not null && collection.Image is not null)
                {
                    FileHelper.FileRemover(collection.Image);
                    collection.Image = null;
                }

                var response = await _httpClient.PutAsJsonAsync(_apiAdres, collection);  // Veriyi Json'a çevirip verilen adrese yolladık.

                if (response.IsSuccessStatusCode) // Api'den başarılı istek kodu geldiyse...
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Hata Oluştu  : " + e.Message);
            }

            var data = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres);

            ViewBag.ParentId = new SelectList(data, "Id", "Name");
            

            return View();
        }

        // GET: CategoriesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Category>(_apiAdres + "/" + id); // Json formatındaki slash işareti kullanımı dolayısıyla / kullandık.
            return View(model);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Category collection)
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
