using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly IService<Category> _service;

        public CategoriesController(IService<Category> service)
        {
            _service = service;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<Category> GetAsync(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<CategoriesController>
        [HttpPost]
        public async Task<Category> Post([FromBody] Category value) // FromQuery = query string ile ...
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();

            return value;
        }

        // PUT api/<CategoriesController>/5
        [HttpPut] // [HttpPut("{id}")] orijinali idi.. Güncellemede id yollamak zorunda kalmadık.
        public async Task<ActionResult> Put(int id, [FromBody] Category value)
        {
            _service.Update(value);
           int sonuc= await _service.SaveAsync();

            if (sonuc > 0)
            {
                return Ok();
            }

            return StatusCode(StatusCodes.Status304NotModified);
        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _service.FindAsync(id);

            if (kayit == null)
            {
                return NotFound();
            }

            _service.Delete(kayit);

            await _service.SaveAsync();

            return Ok();
        }
    }
}
