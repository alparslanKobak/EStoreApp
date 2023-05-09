using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Utils;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IService<Product> _service; // readonly nesneler sadece constructor metotta doldurulabilir.
        private readonly IService<Category> _serviceCategory;
        private readonly IService<Brand> _serviceBrand;

        public ProductController(IService<Product> service, IService<Category> serviceCategory, IService<Brand> serviceBrand)
        {
            _service = service;
            _serviceCategory = serviceCategory;
            _serviceBrand = serviceBrand;

            // Üç servisi de aynı anda construct edebilmesi için üçünü de seçip o şekilde construct etmek gerekir.
        }





        // GET: ProductsController
        public async Task<ActionResult> Index()
        {
            var model = await _service.GetAllAsync();
            
            return View(model);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public async Task<ActionResult> Create()
        {
            var data = await _serviceCategory.GetAllAsync();
            ViewBag.CategoryId = new SelectList(data, "Id", "Name"); 
            
            var data1 = await _serviceBrand.GetAllAsync();
            ViewBag.BrandId = new SelectList(data1, "Id", "Name");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product collection, IFormFile? Image)
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
                ModelState.AddModelError("","Hata Oluştu : " + e.Message);
            }
            var data = await _serviceCategory.GetAllAsync();
            ViewBag.CategoryId = new SelectList(data, "Id", "Name");

            var data1 = await _serviceBrand.GetAllAsync();
            ViewBag.BrandId = new SelectList(data1, "Id", "Name");
            return View();
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
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
