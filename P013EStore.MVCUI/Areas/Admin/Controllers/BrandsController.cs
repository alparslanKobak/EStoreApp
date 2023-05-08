using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly IService<Brand> _service; // readonly nesneler sadece constructor metotta doldurulabilir.

        public BrandsController(IService<Brand> service)
        {
            _service = service;
        }

        // GET: BrandsController
        public ActionResult Index()
        {
            var model = _service.GetAll();

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
                await _service.AddAsync(collection);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) // id gönderilmeden direkt edit sayfası açılırsa 
            {
                return BadRequest(); // geriye geçersiz istek hatası dön
            }

            var model = await _service.FindAsync(id.Value); // Yukarıdaki id'yi ? ile nullable yaparsak 

            if (model==null)
            {
                return NotFound(); // kayıt bulunamadı
            }

            return View(model);



        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Brand collection, IFormFile? Logo, bool? resmiSil)
        {
            try
            {
                if (Logo != null)
                {
                    collection.Logo = await FileHelper.FileLoaderAsync(Logo);
                }


                if (resmiSil== true && resmiSil is not null)
                {
                    collection.Logo = "";
                }

                _service.Update(collection);
                await _service.SaveAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BrandsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BrandsController/Delete/5
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
