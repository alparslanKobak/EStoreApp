using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;
using System.Diagnostics;

namespace P013EStore.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService<Slider> _serviceSlider;
        private readonly IService<Product> _serviceProduct; // Eğer sonradan eklendiyse add parameters to denir.
        private readonly IService<Contact> _serviceContact;
        private readonly IService<News> _serviceNews;
        private readonly IService<Brand> _serviceBrands;
        private readonly IService<Log> _serviceLog;
        private readonly IService<Setting> _serviceSetting;

        public HomeController(IService<Slider> serviceSlider, IService<Product> serviceProduct, IService<Contact> serviceContact, IService<News> serviceNews, IService<Brand> serviceBrands, IService<Log> serviceLog, IService<Setting> serviceSetting)
        {
            _serviceSlider = serviceSlider;
            _serviceProduct = serviceProduct;
            _serviceContact = serviceContact;
            _serviceNews = serviceNews;
            _serviceBrands = serviceBrands;
            _serviceLog = serviceLog;
            _serviceSetting = serviceSetting;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomePageViewModel()
            {
                Sliders = await _serviceSlider.GetAllAsync(),
                Products = await _serviceProduct.GetAllAsync(p => p.IsHome && p.Isactive),
                Brands = await _serviceBrands.GetAllAsync(b => b.IsActive),
                News = await _serviceNews.GetAllAsync(b => b.IsActive && b.IsHome)

            };

            // await _serviceSlider.GetAllAsync();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Route("iletisim")]
        public IActionResult ContactUs()
        {
            //var model = await _serviceSetting.GetAllAsync();

            //if (model != null)
            //{
            //    return View(model.FirstOrDefault());

            //}

            return View();
        }

        [Route("iletisim"), HttpPost]
        public async Task<IActionResult> ContactUs(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceContact.AddAsync(contact);
                    var sonuc = await _serviceContact.SaveAsync();
                    // sonuc kısmı int bir değer döndürür. Eğer -1 ise hatalı, 0'dan büyük ise başarılıdır.

                    if (sonuc > 0)
                    {
                        //   await MailHelper.SendMailAsync(contact); // gelen mesajı mail gönder.
                        TempData["Message"] = "<div class='alert alert-success' > Mesajınız Gönderilmiştir! Teşekkürler... </div>";
                    }

                    return RedirectToAction("ContactUs");
                }
                catch (Exception e)
                {
                    _serviceLog.Add(new Log
                    {
                        // Özelleştirilmiş Log kayıtlarıyla sorunun tam olarak nereden kaynaklandığını kolay bir şekilde öğrenebiliriz.

                        Title = "İletişim Formu Gönderilirken Hata Oluştué",
                        Description = e.Message
                    });
                    await _serviceLog.SaveAsync();

                    // await.MailHelper.SendMailAsync(contact); // Oluşan Hatayı yazılımcıya mail gönder.

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