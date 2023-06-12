using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;

namespace P013EStore.WebAPIUsing.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7011/api/AppUsers";
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {

                TempData["Message"] = "<div class='alert alert-danger' >Lütfen Giriş Yapınız! </div>";

                return RedirectToAction(nameof(Login));


            }
            else
            {

                var user = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + userId);
                return View(user);
            }

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel viewModel)
        {
            var users = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);

            var user = users.FirstOrDefault(x => x.Email == viewModel.Email && x.Password == viewModel.Password && x.Isactive);

            if (user == null)
            {
                ModelState.AddModelError("", "Giriş Başarısız!");
            }
            else
            {
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("userGuid", user.UserGuid.ToString());
            }
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AppUser? appUser)
        {
            if (appUser == null)
            {
                return BadRequest();
            }

            try
            {

                var users = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);

                if (ModelState.IsValid)
                {

                    var kullanici = users.Any(x => x.Email == appUser.Email);

                    if (kullanici != null)
                    {
                        ModelState.AddModelError("", "Bu mail ile kayıt zaten mevcut!");
                        return View();
                    }
                    else
                    {
                        appUser.UserGuid = Guid.NewGuid();
                        appUser.Isactive = true;
                        appUser.IsAdmin = false;
                        var sonuc = await _httpClient.PostAsJsonAsync(_apiAdres, appUser);

                        if (sonuc.IsSuccessStatusCode)
                        {

                            TempData["Message"] = "<div class='alert alert-success' >Kayıt Başarıyla Oluşturuldu... </div>";
                            return RedirectToAction(nameof(Login));

                        }


                    }


                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError("", "Hata oluştu" + e.Message);
            }
            return View(appUser);
        }

        public IActionResult NewPassword()
        {
            return View();
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("userId");
                HttpContext.Session.Remove("userGuid");
            }
            catch
            {

                HttpContext.Session.Clear();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(AppUser? appUser)
        {
            if (appUser == null)
            {
                return BadRequest();
            }

            try
            {
                var userId = HttpContext.Session.GetInt32("userId"); // hidden veri tipi kullanmamak için bu sessionları kullandık

                var user = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + userId);

                if (user != null)
                {
                    user.Name = appUser.Name;

                    user.Surname = appUser.Surname;

                    user.Email = appUser.Email;

                    user.Password = appUser.Password;

                    user.Phone = appUser.Phone;

                    // Bu veri bloğunu asp hidden arealar içerisinde, ön yüz üzerinden hacklenmemek için kullandık.

                    if (ModelState.IsValid)
                    {

                        var response = await _httpClient.PutAsJsonAsync(_apiAdres, appUser);  // Veriyi Json'a çevirip verilen adrese yolladık.

                        if (response.IsSuccessStatusCode) // Api'den başarılı istek kodu geldiyse...
                        {
                        TempData["Message"] = "<div class='alert alert-success' >Hesap Başarıyla Güncellendi... </div>";
                            return RedirectToAction(nameof(Index));
                        }


                    }
                }



            }
            catch (Exception e)
            {

                ModelState.AddModelError("", "Güncelleme Başarısız" + e.Message);
            }

            return View(nameof(Index), appUser);
        }
    }
}
