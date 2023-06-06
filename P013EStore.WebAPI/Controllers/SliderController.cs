using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliderController : ControllerBase
    {
        private readonly IService<Slider> _service;

        public SliderController(IService<Slider> service)
        {
            _service = service;
        }

        // GET: api/<SliderController>
        [HttpGet]
        public async Task<IEnumerable<Slider>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<SliderController>/5
        [HttpGet("{id}")]
        public async Task<Slider> Get(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<SliderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Slider value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();

            return Ok(value);
        }

        // PUT api/<SliderController>/5
        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody] Slider value)
        {
            _service.Update(await _service.FindAsync(id));

            await _service.SaveAsync();

            return Ok(value);
        }

        // DELETE api/<SliderController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            _service.Delete(await _service.FindAsync(id));

            var sonuc = await _service.SaveAsync();

            if (sonuc > 0)
            {
                return Ok();

            }

            return Problem();

        }
    }
}
