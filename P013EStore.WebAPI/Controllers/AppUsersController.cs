using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

// Controller oluşturulurken read/write actions sayfasında sol tarafta bulunulan API tools seçildi.

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {

        private readonly IService<AppUser> _service;

        public AppUsersController(IService<AppUser> service)
        {
            _service = service;
        }

        // GET: api/<AppUsersController>
        [HttpGet]
        public async Task<IEnumerable<AppUser>> Get() // Listeleme metodu
        {
            return await _service.GetAllAsync();
        }

        // GET api/<AppUsersController>/5
        [HttpGet("{id}")]
        public async Task<AppUser> Get(int id) // Getirme metodu
        {
            return await _service.FindAsync(id);
        }

        // POST api/<AppUsersController>
        [HttpPost]
        public async Task Post([FromBody] AppUser value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
        }

        // PUT api/<AppUsersController>/5
        [HttpPut]
        public async Task Put(int id, [FromBody] AppUser value)
        {
            _service.Update(value);
            await _service.SaveAsync();
        }

        // DELETE api/<AppUsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var kayit = await _service.FindAsync(id);

            if (kayit == null)
            {
                return BadRequest();
            }

            _service.Delete(kayit);
            await _service.SaveAsync();
            return Ok(kayit);
        }
    }
}
