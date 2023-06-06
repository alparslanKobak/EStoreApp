using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Utils;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly HttpClient _httpClient; // _httpClient nesnesini kullanarak api lere istek gönderebiliriz.

        public BrandsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7011/api/Brands"; //Api adresini web api projesini çalıştırdığımızda adres çubuğundan veya herhangi bir controller'a istek atarak RequestURL kısmından veya web api projesinde Properties altındaki launchSettings.json 

        // GET: BrandsController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Brand>>(_apiAdres); // _httpClient nesnesi içindeki GetFromJsonAsync metodu kendisine verdiğimiz _apiAdres'deki URL'e get isteği gönderir ve oradan gelen json formatındaki app user listesini List<AppUser> nesnesine dönüştürür.

            return View(model);
        }

        // GET: BrandsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Brand collection, IFormFile? Logo)
        {
            try
            {
                if (Logo != null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }

                var response = await _httpClient.PostAsJsonAsync(_apiAdres, collection);  // Veriyi Json'a çevirip verilen adrese yolladık.

                if (response.IsSuccessStatusCode) // Api'den başarılı istek kodu geldiyse...
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Hata Oluştu" + e.Message);
            }

            return View(collection);
        }

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Category>(_apiAdres + "/" + id); // Json formatındaki slash işareti kullanımı dolayısıyla / kullandık.
            return View(model);
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Brand collection, IFormFile? Logo, bool? resmiSil)
        {
            try
            {
                if (Logo != null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }


                if (resmiSil == true && resmiSil is not null && collection.Logo is not null)
                {
                    FileHelper.FileRemover(collection.Logo);
                    collection.Logo = null;
                }

                var response = await _httpClient.PutAsJsonAsync(_apiAdres, collection);  // Veriyi Json'a çevirip verilen adrese yolladık.

                if (response.IsSuccessStatusCode) // Api'den başarılı istek kodu geldiyse...
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Hata Oluştu : " + e.Message);
            }
            return View();
        }

        // GET: BrandsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Category>(_apiAdres + "/" + id); // Json formatındaki slash işareti kullanımı dolayısıyla / kullandık.
            return View(model);
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Brand collection)
        {
            try
            {
                if (collection.Logo is not null)
                {
                    FileHelper.FileRemover(collection.Logo);

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
