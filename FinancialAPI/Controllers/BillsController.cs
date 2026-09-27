using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialAPI.Services;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(ulong id)
        {
            var item = await _svc.GetAsync(User, id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("{cardId}/recalculate")]
        public async Task<IActionResult> Recalculate(ulong cardId)
        {
            var ok = await _svc.RecalculateAsync(User, cardId);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
