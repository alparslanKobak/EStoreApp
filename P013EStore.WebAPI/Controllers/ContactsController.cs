using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

namespace P013EStore.WebAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {

        private readonly IService<Contact> _service;

        public ContactsController(IService<Contact> service)
        {
            _service = service;
        }

        // GET: api/<ContactsController>
        [HttpGet]
        public async Task<IEnumerable<Contact>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public async Task<Contact> Get(int id)
        {
            return await _service.FindAsync(id) ;
        }

        // POST api/<ContactsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Contact value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();

            return Ok(value);
        }

        // PUT api/<ContactsController>/5
        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody] Contact value)
        {
            _service.Update(value);
            await _service.SaveAsync();


            // Geriye değer dönebilmek için actionresult gibi bir imzayı çağırdık.

            return Ok(value);
        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _service.Delete(await _service.FindAsync(id));
            await _service.SaveAsync();

            return Ok();
        }
    }
}
