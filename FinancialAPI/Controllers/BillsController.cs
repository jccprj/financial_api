using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialAPI.Services;
using FinancialAPI.Dtos;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("api/bills")]
    [Authorize]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _svc;

        public BillsController(IBillService svc)
        {
            _svc = svc;
        }

        /// <summary>
        /// Obtém uma fatura por identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BillDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(ulong id)
        {
            var item = await _svc.GetAsync(User, id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Recalcula as faturas de um cartão de crédito.
        /// </summary>
        [HttpPost("{cardId}/recalculate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Recalculate(ulong cardId)
        {
            var ok = await _svc.RecalculateAsync(User, cardId);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
