using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;
using System.Security.Claims;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7011/api/AppUsers";

        public IActionResult Index(string ReturnUrl)
        {
            var model = new AdminLoginViewModel();
            model.ReturnUrl = ReturnUrl;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Index(AdminLoginViewModel adminLoginViewModel)
        {
            try
            {
                var userList = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);

                var account = userList.FirstOrDefault(x=> x.Email == adminLoginViewModel.Email && x.Password == adminLoginViewModel.Password && x.Isactive); // Eğer email şifre ve hesap aktifliği uyumlu ve uygun ise

                if (account == null) // Eğer istenen şartlar sağlanmazsa Giriş yapılamasın
                {
                    ModelState.AddModelError("","Giriş Başarısız ! ");
                }
                else
                {
                    List<Claim> kullaniciYetkileri = new List<Claim>
                    { new Claim(ClaimTypes.Email,account.Email),

                    new Claim("Role",account.IsAdmin? "Admin" : "User"),

                    new Claim("UserGuid",account.UserGuid.ToString())

                    };

                    ClaimsIdentity userID = new ClaimsIdentity(kullaniciYetkileri, "Login");

                    ClaimsPrincipal principal = new(userID);

                    await HttpContext.SignInAsync(principal);
                }

                return Redirect(string.IsNullOrEmpty(adminLoginViewModel.ReturnUrl) ? "/Admin/Main" : adminLoginViewModel.ReturnUrl); // Eğer return Url kısmı boşsa Admin/Main'e git >> Aksi takdirde return url üzerinde belirtilen noktaya dön

            }
            catch (Exception e)
            {

                ModelState.AddModelError("", "Hata Oluştu : " + e.Message);
            }
           
            return View(adminLoginViewModel);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Admin/Login");
        }
    }
}
