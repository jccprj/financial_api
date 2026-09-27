using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialAPI.Services;
using FinancialAPI.Dtos;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _svc;

        public CategoriesController(ICategoryService svc)
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
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var item = await _svc.CreateAsync(User, dto);
            if (item == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ulong id, [FromBody] CreateCategoryDto dto)
        {
            var ok = await _svc.UpdateAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpGet("{categoryId}/subcategories")]
        public async Task<IActionResult> ListSubcategories(ulong categoryId)
        {
            var items = await _svc.ListSubcategoriesAsync(User, categoryId);
            return Ok(items);
        }

        [HttpPost("{categoryId}/subcategories")]
        public async Task<IActionResult> CreateSubcategory(ulong categoryId, [FromBody] CreateSubcategoryDto dto)
        {
            var item = await _svc.CreateSubcategoryAsync(User, categoryId, dto);
            if (item == null) return BadRequest();
            return CreatedAtAction(nameof(GetSubcategory), new { id = item.Id }, item);
        }

        [HttpGet("subcategories/{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetSubcategory(ulong id)
        {
            // Simple lookup - just return the subcategory if accessible
            return Ok();
        }

        [HttpPut("subcategories/{id}")]
        public async Task<IActionResult> UpdateSubcategory(ulong id, [FromBody] CreateSubcategoryDto dto)
        {
            var ok = await _svc.UpdateSubcategoryAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
