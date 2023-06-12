using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class AppUsersController : Controller
    {

        private readonly HttpClient _httpClient; // _httpClient nesnesini kullanarak api lere istek gönderebiliriz.


        private readonly string _apiAdres = "https://localhost:7011/api/AppUsers"; //Api adresini web api projesini çalıştırdığımızda adres çubuğundan veya herhangi bir controller'a istek atarak RequestURL kısmından veya web api projesinde Properties altındaki launchSettings.json 

        public AppUsersController(HttpClient httpClient)
        {
            _httpClient = httpClient; // _httpClient nesnesinin apiye ulaşması için api projesinin de bu projeyle birlikte çalışıyor olması lazım!! 

            // Aynı anda 2 projeyi çalıştırabilmek için Solution'a sağ tıklayıp açılan menüden Configure startup projects diyip açılan ekrandan multiple alanına tıklayıp aynı anda başlatmak istediğimiz projeleri seçiyoruz!
        }

        // GET: AppUsersController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres); // _httpClient nesnesi içindeki GetFromJsonAsync metodu kendisine verdiğimiz _apiAdres'deki URL'e get isteği gönderir ve oradan gelen json formatındaki app user listesini List<AppUser> nesnesine dönüştürür.

            return View(model);
        }

        // GET: AppUsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppUsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppUsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppUser collection)
        {
            try
            {
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

        // GET: AppUsersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + id); // Json formatındaki slash işareti kullanımı dolayısıyla / kullandık.
            return View(model);
        }

        // POST: AppUsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AppUser collection)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(_apiAdres, collection);  // Veriyi Json'a çevirip verilen adrese yolladık.

                if (response.IsSuccessStatusCode) // Api'den başarılı istek kodu geldiyse...
                {
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch(Exception e)
            {
                ModelState.AddModelError("","Hata Oluştu : " + e.Message);
            }
                return View();
        }

        // GET: AppUsersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + id); // Json formatındaki slash işareti kullanımı dolayısıyla / kullandık.
            return View(model);
        }

        // POST: AppUsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, AppUser collection)
        {
            try
            {
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
