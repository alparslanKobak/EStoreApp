using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Utils;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly HttpClient _httpClient;

        public SliderController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        private readonly string _apiAdres = "https://localhost:7011/api/Slider";
        // GET: SliderController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Slider>>(_apiAdres); // _httpClient nesnesi içindeki GetFromJsonAsync metodu kendisine verdiğimiz _apiAdres'deki URL'e get isteği gönderir ve oradan gelen json formatındaki app user listesini List<AppUser> nesnesine dönüştürür.

            return View(model);
        }

        // GET: SliderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SliderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Slider collection, IFormFile? Image)
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
            catch (Exception e)
            {
                ModelState.AddModelError("", "Hata Oluştu  : " + e.Message);
            }

            return View();
        }

        // GET: SliderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SliderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SliderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
