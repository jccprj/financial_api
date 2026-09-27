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

        [HttpPut("{id}/classify")]
        public async Task<IActionResult> UpdateClassification(ulong id, [FromBody] MerchantDto dto)
        {
            var ok = await _svc.UpdateClassificationAsync(User, id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
