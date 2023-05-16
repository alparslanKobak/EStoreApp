using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;
using System.Security.Claims;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IService<AppUser> _service;

        public LoginController(IService<AppUser> service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AdminLoginViewModel admin)
        {
            try
            {
                AppUser user = await _service.GetAsync(x => x.Isactive && x.Email == admin.Email && x.Password == admin.Password);

                if (user != null)
                {

                    List<Claim> kullaniciYetkileri = new List<Claim>
                    { new Claim(ClaimTypes.Email,user.Email),

                    new Claim("Role",user.IsAdmin? "Admin" : "User"),

                    new Claim("UserGuid",user.UserGuid.ToString())

                    };

                    ClaimsIdentity userID = new ClaimsIdentity(kullaniciYetkileri, "Login");

                    ClaimsPrincipal principal = new(userID);

                    await HttpContext.SignInAsync(principal); // HttpContext .net içerisinden gelmektedir ve uygulamanın çalışma anındaki içeriğe ulaşmamızı sağlıyor. SignInAsync metodu da .net içerisinde mevcut login işlemi yapmak istersek buradaki gibi kullanıyoruz.

                    return Redirect("/Admin/Main");
                }
                else
                {
                    ModelState.AddModelError("", "Giriş Başarısız!" );

                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError("", "Hata Oluştu : " + e.Message);
            }
            return View();

        }
        [Route("Logout")] // Çıkış işlemi için eklemeyi unutmamalısın
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // Sistemden çıkış yap.

            return Redirect("/Admin/Login"); // Tekrardan giriş sayfasına yönlendir.
            
        }
    }
}
