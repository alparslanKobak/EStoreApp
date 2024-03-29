﻿using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.Service.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P013EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IService<Log> _service;

        public LogsController(IService<Log> service)
        {
            _service = service;
        }
        // GET: api/<LogsController>
        [HttpGet]
        public async Task<IEnumerable<Log>> Get()
        {
            var model = await _service.GetAllAsync();

            return model;
        }

        // GET api/<LogsController>/5
        [HttpGet]
        public async Task<Log> Get(int id)
        {
            return await _service.FindAsync(id);
        }

        // POST api/<LogsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Log value)
        {
            await _service.AddAsync(value);

            await _service.SaveAsync();

            return Ok(value);
        }

        // PUT api/<LogsController>/5
        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody] Log value)
        {
            _service.Update(value);
            await _service.SaveAsync();
            return Ok(value);
        }

        // DELETE api/<LogsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _service.Delete(await _service.FindAsync(id));
            await _service.SaveAsync();

            return Ok(); // Sürekli ok dönmek yanlış işlem gerçekleştiğinde hata verdirebilir.
        }
    }
}
