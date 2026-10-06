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

        /// <summary>
        /// Lista todas as categorias do usuário.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List()
        {
            var items = await _svc.ListAsync(User);
            return Ok(items);
        }

        /// <summary>
        /// Obtém categoria por identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(ulong id)
        {
            var item = await _svc.GetAsync(User, id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Cria uma nova categoria.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var item = await _svc.CreateAsync(User, dto);
            if (item == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(ulong id, [FromBody] CreateCategoryDto dto)
        {
            var ok = await _svc.UpdateAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Lista subcategorias de uma categoria.
        /// </summary>
        [HttpGet("{categoryId}/subcategories")]
        [ProducesResponseType(typeof(IEnumerable<SubcategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListSubcategories(ulong categoryId)
        {
            var items = await _svc.ListSubcategoriesAsync(User, categoryId);
            return Ok(items);
        }

        /// <summary>
        /// Cria uma subcategoria para uma categoria.
        /// </summary>
        [HttpPost("{categoryId}/subcategories")]
        [ProducesResponseType(typeof(SubcategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
            return Ok();
        }

        /// <summary>
        /// Atualiza uma subcategoria.
        /// </summary>
        [HttpPut("subcategories/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSubcategory(ulong id, [FromBody] CreateSubcategoryDto dto)
        {
            var ok = await _svc.UpdateSubcategoryAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
