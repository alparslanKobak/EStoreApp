using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]

    public class SliderController : Controller
    {
        private readonly IService<Slider> _service;

        public SliderController(IService<Slider> service)
        {
            _service = service;
        }

        // GET: SliderController
        public async Task<ActionResult> Index()
        {
            var model = await _service.GetAllAsync();
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
                await _service.AddAsync(collection);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var data = await _service.GetAllAsync();
                ViewBag.ParentId = new SelectList(data, "Id", "Name");
                return View();
            }
        }

        // GET: SliderController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) // id gönderilmeden direkt edit sayfası açılırsa 
            {
                return BadRequest(); // geriye geçersiz istek hatası dön
            }

            var model = await _service.FindAsync(id.Value); // Yukarıdaki id'yi ? ile nullable yaparsak 

            if (model == null)
            {
                return NotFound(); // kayıt bulunamadı
            }

            return View(model);
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Slider collection, IFormFile? Image, bool? resmiSil)
        {

            try
            {
                if (Image != null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                }


                if (resmiSil == true && resmiSil is not null && collection.Image is not null)
                {
                    FileHelper.FileRemover(collection.Image);
                    collection.Image = null;
                }
                // Sunucudan tamamen dosya silme durumunu araştır.
                _service.Update(collection);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var data = await _service.GetAllAsync();
                ViewBag.ParentId = new SelectList(data, "Id", "Name");
                return View();
            }
        }

        // GET: SliderController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Slider model = await _service.FindAsync(id.Value);
            return View();
        }

        // POST: SliderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Slider collection)
        {
            try
            {
                _service.Delete(collection);
                 await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
