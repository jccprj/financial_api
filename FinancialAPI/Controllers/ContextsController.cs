using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialAPI.Services;
using FinancialAPI.Dtos;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContextsController : ControllerBase
    {
        private readonly IContextService _svc;

        public ContextsController(IContextService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var items = await _svc.ListAsync(User);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ulong id)
        {
            var item = await _svc.GetAsync(User, id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContextDto dto)
        {
            var item = await _svc.CreateAsync(User, dto);
            if (item == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ulong id, [FromBody] CreateContextDto dto)
        {
            var ok = await _svc.UpdateAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
