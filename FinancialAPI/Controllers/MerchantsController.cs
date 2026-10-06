using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialAPI.Services;
using FinancialAPI.Dtos;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MerchantsController : ControllerBase
    {
        private readonly IMerchantService _svc;

        public MerchantsController(IMerchantService svc)
        {
            _svc = svc;
        }

        /// <summary>
        /// Lista estabelecimentos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MerchantDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List()
        {
            var items = await _svc.ListAsync(User);
            return Ok(items);
        }

        /// <summary>
        /// Obtém estabelecimento por identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MerchantDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(ulong id)
        {
            var item = await _svc.GetAsync(User, id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Atualiza a classificação de um estabelecimento.
        /// </summary>
        [HttpPut("{id}/classify")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateClassification(ulong id, [FromBody] MerchantDto dto)
        {
            var ok = await _svc.UpdateClassificationAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
