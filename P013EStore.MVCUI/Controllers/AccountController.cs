using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<AppUser> _service;

        public AccountController(IService<AppUser> service)
        {
            _service = service;
        }

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
                AppUser user = await _service.GetAsync(u => u.Id == userId);
                return View(user);
            }
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

                AppUser user = await _service.GetAsync(u => u.Id == userId);

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

                        _service.Update(user);
                        await _service.SaveAsync();

                        TempData["Message"] = "<div class='alert alert-success' >Hesap Başarıyla Güncellendi... </div>";

                        return RedirectToAction(nameof(Index));
                    }
                }


               
            }
            catch (Exception e)
            {

                ModelState.AddModelError("", "Güncelleme Başarısız" + e.Message);
            }

            return View(nameof(Index),appUser);
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel viewModel)
        {
            AppUser user = await _service.GetAsync(x => x.Email == viewModel.Email && x.Password == viewModel.Password && x.Isactive);

            if (user == null)
            {
                ModelState.AddModelError("", "Giriş Başarısız");


            }
            else
            { // Session kullanmak için önce program.cs ile koordineli çalıştık.
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("userGuid", user.UserGuid.ToString());

                return RedirectToAction(nameof(Index));
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
                if (ModelState.IsValid)
                {

                    AppUser kullanici = await _service.GetAsync(x => x.Email == appUser.Email);

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
                        await _service.AddAsync(appUser);
                        await _service.SaveAsync();

                        TempData["Message"] = "<div class='alert alert-success' >Kayıt Başarıyla Oluşturuldu... </div>";

                        return RedirectToAction(nameof(Login));
                    }


                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError("", "Hata oluştu" + e.Message);
            }


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
            
            return RedirectToAction("Index","Home");
        }

        public IActionResult NewPassword()
        {
            return View();
        }
    }
}
