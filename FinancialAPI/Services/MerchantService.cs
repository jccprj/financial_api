using FinancialAPI.Data;
using FinancialAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinancialAPI.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly ApplicationDbContext _db;

        public MerchantService(ApplicationDbContext db)
        {
            _db = db;
        }

        private ulong? GetUserId(ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ulong.TryParse(sub, out var id)) return id;
            return null;
        }

        public async Task<IEnumerable<MerchantDto>> ListAsync(ClaimsPrincipal user)
        {
            var userId = GetUserId(user);
            if (userId == null) return Enumerable.Empty<MerchantDto>();

            var items = await _db.Merchants.Where(m => m.Active).ToListAsync();
            return items.Select(m => new MerchantDto(m.Id, m.Name, m.RequiresClassification));
        }

        public async Task<MerchantDto?> GetAsync(ClaimsPrincipal user, ulong id)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var item = await _db.Merchants.FirstOrDefaultAsync(m => m.Id == id && m.Active);
            if (item == null) return null;
            return new MerchantDto(item.Id, item.Name, item.RequiresClassification);
        }

        public async Task<bool> UpdateClassificationAsync(ClaimsPrincipal user, ulong id, MerchantDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return false;

            var merchant = await _db.Merchants.FindAsync(id);
            if (merchant == null) return false;

            merchant.RequiresClassification = dto.RequiresClassification;
            merchant.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
