using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P013EStore.Core.Entities;
using P013EStore.Data;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]

    public class CategoriesController : Controller
    {
        private readonly IService<Category> _service;

        public CategoriesController(IService<Category> service)
        {
            _service = service;
        }

        // GET: CategoriesController
        public async Task<ActionResult> Index()
        {
            var model = await _service.GetAllAsync();
            return View(model);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriesController/Create
        public async Task<ActionResult> Create()
        {
            var data = await _service.GetAllAsync();
            ViewBag.ParentId = new SelectList(data,"Id","Name");
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category collection, IFormFile? Image)
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

        // GET: CategoriesController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
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

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Category collection, IFormFile? Image, bool? resmiSil)
        {
            
            try
            {

                if (resmiSil == true && resmiSil is not null && collection.Image is not null)
                {
                    FileHelper.FileRemover(collection.Image);
                    collection.Image = null;
                }

                if (Image != null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
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

        // GET: CategoriesController/Delete/5
        public async Task<ActionResult> DeleteAsync(int? id)
        {
            if (id == null) // id gönderilmeden direkt edit sayfası açılırsa 
            {
                return BadRequest(); // geriye geçersiz istek hatası dön
            }
            var model = await _service.FindAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category collection)
        {
            try
            {
                if (collection.Image!= null)
                {
                    FileHelper.FileRemover(collection.Image);
                }
                
                _service.Delete(collection);
                _service.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
