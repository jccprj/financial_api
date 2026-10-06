using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialAPI.Services;
using FinancialAPI.Dtos;
using System.ComponentModel;

namespace FinancialAPI.Controllers
{
    [ApiController]
    [Route("api/credit-cards")]
    [Authorize]
    public class CreditCardsController : ControllerBase
    {
        private readonly ICardService _svc;
        private readonly IBillService _billSvc;

        public CreditCardsController(ICardService svc, IBillService billSvc)
        {
            _svc = svc;
            _billSvc = billSvc;
        }

        /// <summary>
        /// Lista os cartões de crédito do usuário.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CardDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List()
        {
            var items = await _svc.ListAsync(User);
            return Ok(items);
        }

        /// <summary>
        /// Obtém um cartão por identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(ulong id)
        {
            var item = await _svc.GetAsync(User, id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        /// <summary>
        /// Cria um novo cartão de crédito.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CardDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateCardDto dto)
        {
            var item = await _svc.CreateAsync(User, dto);
            if (item == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        /// <summary>
        /// Atualiza os dados de um cartão.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(ulong id, [FromBody] UpdateCardDto dto)
        {
            var ok = await _svc.UpdateAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Atualiza o dia de vencimento e recalcula as faturas.
        /// </summary>
        [HttpPatch("{id}/due-day")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDueDay(ulong id, [FromBody] UpdateDueDayDto dto)
        {
            var ok = await _svc.UpdateDueDayAsync(User, id, dto.DueDay);
            if (!ok) return NotFound();

            await _billSvc.RecalculateAsync(User, id);

            return NoContent();
        }

        /// <summary>
        /// Lista as faturas de um cartão.
        /// </summary>
        [HttpGet("{cardId}/bills")]
        [ProducesResponseType(typeof(IEnumerable<BillDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListBills(ulong cardId)
        {
            var items = await _billSvc.ListByCardAsync(User, cardId);
            return Ok(items);
        }
    }

    public record UpdateDueDayDto(
        [property: Description("Novo dia de vencimento da fatura"), DefaultValue((byte)10)] byte? DueDay);
}
