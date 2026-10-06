using Microsoft.AspNetCore.Mvc;
using FinancialAPI.Services;
using FinancialAPI.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _svc;

        public TransactionsController(ITransactionService svc)
        {
            _svc = svc;
        }

        /// <summary>
        /// Lista transações com filtros opcionais.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, [FromQuery] decimal? minAmount, [FromQuery] decimal? maxAmount, [FromQuery] ulong? merchantId, [FromQuery] ulong? categoryId, [FromQuery] string? status)
        {
            var filters = new TransactionFilterParams(dateFrom, dateTo, minAmount, maxAmount, merchantId, categoryId, status);
            var items = await _svc.ListAsync(User, filters);
            return Ok(items);
        }

        /// <summary>
        /// Lista transações pendentes de classificação.
        /// </summary>
        [HttpGet("pending-classifications")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Pending()
        {
            var items = await _svc.ListPendingClassificationsAsync(User);
            return Ok(items);
        }

        /// <summary>
        /// Obtém uma transação por identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(ulong id)
        {
            var item = await _svc.GetAsync(User, id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Classifica manualmente uma transação.
        /// </summary>
        [HttpPut("{id}/classify")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Classify(ulong id, [FromBody] ClassifyTransactionDto dto)
        {
            var ok = await _svc.ClassifyAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
