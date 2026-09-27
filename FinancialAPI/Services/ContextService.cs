using FinancialAPI.Data;
using FinancialAPI.Dtos;
using FinancialAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinancialAPI.Services
{
    public class ContextService : IContextService
    {
        private readonly ApplicationDbContext _db;

        public ContextService(ApplicationDbContext db)
        {
            _db = db;
        }

        private ulong? GetUserId(ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ulong.TryParse(sub, out var id)) return id;
            return null;
        }

        public async Task<IEnumerable<ContextDto>> ListAsync(ClaimsPrincipal user)
        {
            var userId = GetUserId(user);
            if (userId == null) return Enumerable.Empty<ContextDto>();

            var items = await _db.Contexts.Where(c => c.UserId == userId.Value && c.Active).ToListAsync();
            return items.Select(c => new ContextDto(c.Id, c.Name));
        }

        public async Task<ContextDto?> GetAsync(ClaimsPrincipal user, ulong id)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var item = await _db.Contexts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);
            if (item == null) return null;
            return new ContextDto(item.Id, item.Name);
        }

        public async Task<ContextDto?> CreateAsync(ClaimsPrincipal user, CreateContextDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var ctx = new ContextEntity
            {
                UserId = userId.Value,
                Name = dto.Name,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Contexts.Add(ctx);
            await _db.SaveChangesAsync();
            return new ContextDto(ctx.Id, ctx.Name);
        }

        public async Task<bool> UpdateAsync(ClaimsPrincipal user, ulong id, CreateContextDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return false;

            var ctx = await _db.Contexts.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);
            if (ctx == null) return false;

            ctx.Name = dto.Name;
            ctx.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
