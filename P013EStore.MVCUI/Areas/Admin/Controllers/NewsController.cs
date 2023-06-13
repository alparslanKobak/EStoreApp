using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class NewsController : Controller
    {
        private readonly IService<News> _service;

        public NewsController(IService<News> service)
        {
            _service = service;
        }

        // GET: NewsController
        public async Task<ActionResult> Index()
        {
            IEnumerable<News> news = await _service.GetAllAsync();
            return View(news);
        }

        // GET: NewsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(News collection, IFormFile? Image)
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
            catch(Exception e)
            {
                ModelState.AddModelError("","Hata oluştu : " + e.Message);
            }
                return View();
        }

        // GET: NewsController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) // id gönderilmeden direkt edit sayfası açılırsa 
            {
                return BadRequest(); // geriye geçersiz istek hatası dön
            }

            News model = await _service.FindAsync(id.Value); // Yukarıdaki id'yi ? ile nullable yaparsak 

            if (model == null)
            {
                return NotFound(); // kayıt bulunamadı
            }

            return View(model);
        }

        // POST: NewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, News collection, IFormFile? Image, bool resmiSil)
        {
            try
            {
                if (Image != null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);

                }

                _service.Update(collection);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Hata oluştu : " + e.Message);
            }
            return View();
        }

        // GET: NewsController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) // id gönderilmeden direkt edit sayfası açılırsa 
            {
                return BadRequest(); // geriye geçersiz istek hatası dön
            }
            News news =await _service.FindAsync(id.Value);

            if (news == null)
            {
                return NotFound(); // kayıt bulunamadı
            }
            return View(news);
        }

        // POST: NewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, News collection)
        {
            try
            {
                if (collection.Image != null)
                {
                    FileHelper.FileRemover(collection.Image);
                }

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
