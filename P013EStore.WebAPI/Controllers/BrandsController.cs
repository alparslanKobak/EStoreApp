using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;


namespace P013EStore.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        private readonly IService<Brand> _service; // readonly nesneler sadece constructor metotta doldurulabilir.

        public BrandsController(IService<Brand> service)
        {
            _service = service;
        }
        // GET: api/<BrandsController>
        [HttpGet]
        public async Task<IEnumerable<Brand>> Get()
        {
            return await _service.GetAllAsync();
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public async Task<Brand> Get(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<BrandsController>
        [HttpPost]
        public async Task<Brand> Post([FromBody] Brand value)
        {
            await _service.AddAsync(value);
            await _service.SaveAsync();
            return value;
        }

        // PUT api/<BrandsController>/5
        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] Brand value)
        {
                // ActionResult ile, geriye işlemin başarılı olup olmadığına dair değer döndürdük.

            _service.Update(value);
            int sonuc = await _service.SaveAsync();

            if (sonuc > 0)
            {

                return Ok(value);  // Hatalı ise -1 değeri döndürür.
            }

            return StatusCode(StatusCodes.Status304NotModified); // Yapılan işlem sonucunda herhangi bir değişiklik olmadı.
        }

        // DELETE api/<BrandsController>/5
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

            return Ok(kayit);
        }
    }
}
