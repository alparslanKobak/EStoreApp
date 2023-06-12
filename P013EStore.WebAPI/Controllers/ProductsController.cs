using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {

            return await _service.GetProductsByIncludeAsync();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<Product> Get(int id)
        {
            return await _service.GetProductByIncludeAsync(id);
        }

        [HttpGet("GetSearch/{q}")]
        public async Task<IEnumerable<Product>> GetSearch(string q)
        {
            var model = _service.GetProductsByIncludeAsync(p => p.Isactive && p.Name.Contains(q) || p.Name.Contains(q) || p.Description.Contains(q) || p.Brand.Name.Contains(q) || p.Category.Name.Contains(q));

            
            return await _service.GetProductsByIncludeAsync();
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Product value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();

            return Ok(value);

        }

        // PUT api/<ProductsController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Product value)
        {
            _service.Update(value);
          var sonuc =  await _service.SaveAsync();

            if (sonuc >0)
            {
                return Ok(value);
            }

            return Problem(); // Problem olduğunu belirten metod

        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {


           var data = await _service.FindAsync(id);
            if (data == null)
            {
                return NotFound(data);
            }

             _service.Delete(data);

           var sonuc = await _service.SaveAsync();

            if (sonuc > 0)
            {
                return Ok(data);
            }

            return Problem();
        }
    }
}
