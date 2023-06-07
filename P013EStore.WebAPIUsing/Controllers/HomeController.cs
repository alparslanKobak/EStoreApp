using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;
using System.Diagnostics;

namespace P013EStore.WebAPIUsing.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiAdres = "https://localhost:7011/api/";
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "Products");
            var model = new HomePageViewModel()
            {
                Sliders = await _httpClient.GetFromJsonAsync<List<Slider>>(_apiAdres),

                Products = products.Where(p => p.Isactive && p.IsHome).ToList()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("iletisim")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [Route("iletisim"), HttpPost]
        public async Task<IActionResult> ContactUs(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "Contacts", contact);

                    // sonuc kısmı int bir değer döndürür. Eğer -1 ise hatalı, 0'dan büyük ise başarılıdır.

                    if (response.IsSuccessStatusCode)
                    {
                        //   await MailHelper.SendMailAsync(contact); // gelen mesajı mail gönder.
                        TempData["Message"] = "<div class='alert alert-success' > Mesajınız Gönderilmiştir! Teşekkürler... </div>";
                        
                        return RedirectToAction("ContactUs");
                    }

                }
                catch (Exception e)
                {

                    ModelState.AddModelError("", "Hata Oluştu!" + e.Message);
                }
            }
            return View();
        }

        [Route("AccessDenied")] // AccessDenied hatası sayfası için bir view tasarlamamız gereklidir
        public IActionResult AccessDenied() // Erişim engeli
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}