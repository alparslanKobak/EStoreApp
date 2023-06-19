using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Utils;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class SettingsController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiAdres = "https://localhost:7011/api/Settings"; //Api adresini web api projesini çalıştırdığımızda adres çubuğundan veya herhangi bir controller'a istek atarak RequestURL kısmından veya web api projesinde Properties altındaki launchSettings.json 


        public SettingsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // GET: SettingsController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Setting>>(_apiAdres); // _httpClient nesnesi içindeki GetFromJsonAsync metodu kendisine verdiğimiz _apiAdres'deki URL'e get isteği gönderir ve oradan gelen json formatındaki app user listesini List<AppUser> nesnesine dönüştürür.

            return View(model);
        }

        // GET: SettingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SettingsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SettingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Setting collection, IFormFile? Logo, IFormFile? Favicon)
        {
            try
            {
                if (Logo != null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }

                if (Favicon != null)
                {
                    collection.Favicon = await FileHelper.FileLoaderAsync(Favicon);
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

            return View(collection);
        }

        // GET: SettingsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Setting>(_apiAdres + "/" + id); // Json formatındaki slash işareti kullanımı dolayısıyla / kullandık.
            return View(model);
        }

        // POST: SettingsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Setting collection, IFormFile? Logo, bool? logoyuSil, bool? faviconuSil, IFormFile? Favicon)
        {
            try
            {

                if (logoyuSil == true && logoyuSil is not null && collection.Logo is not null)
                {
                    FileHelper.FileRemover(collection.Logo);
                    collection.Logo = null;
                }

                if (Logo != null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }

                if (faviconuSil == true && collection.Favicon is not null)
                {
                     FileHelper.FileRemover(collection.Favicon);

                    collection.Favicon = null;


                }

                if (Favicon != null)
                {
                    collection.Favicon = await FileHelper.FileLoaderAsync(Favicon);
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

        // GET: SettingsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Setting>(_apiAdres + "/" + id); // Json formatındaki slash işareti kullanımı dolayısıyla / kullandık.
            return View(model);
        }

        // POST: SettingsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Setting collection)
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
