using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialAPI.Services;
using FinancialAPI.Dtos;

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
        public async Task<IActionResult> Create([FromBody] CreateCardDto dto)
        {
            var item = await _svc.CreateAsync(User, dto);
            if (item == null) return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(ulong id, [FromBody] UpdateCardDto dto)
        {
            var ok = await _svc.UpdateAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/due-day")]
        public async Task<IActionResult> UpdateDueDay(ulong id, [FromBody] UpdateDueDayDto dto)
        {
            var ok = await _svc.UpdateDueDayAsync(User, id, dto.DueDay);
            if (!ok) return NotFound();

            // Trigger bill recalculation
            await _billSvc.RecalculateAsync(User, id);

            return NoContent();
        }

        [HttpGet("{cardId}/bills")]
        public async Task<IActionResult> ListBills(ulong cardId)
        {
            var items = await _billSvc.ListByCardAsync(User, cardId);
            return Ok(items);
        }
    }

    public record UpdateDueDayDto(byte? DueDay);
}
