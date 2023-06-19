using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly IService<News> _service;

        public NewsController(IService<News> service)
        {
            _service = service;
        }
        // GET: api/<NewsController>
        [HttpGet]
        public async Task<IEnumerable<News>> Get()
        {
            var model = await _service.GetAllAsync();

            return model;
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public async Task<News> Get(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<NewsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] News value)
        {
            await _service.AddAsync(value);

            await _service.SaveAsync();

            return Ok(value);
        }

        // PUT api/<NewsController>/5
        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody] News value)
        {
            _service.Update(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _service.Delete(await _service.FindAsync(id));
            await _service.SaveAsync();

            return Ok(); // Sürekli ok dönmek yanlış işlem gerçekleştiğinde hata verdirebilir.
        }
    }
}
