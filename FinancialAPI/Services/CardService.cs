using FinancialAPI.Data;
using FinancialAPI.Dtos;
using FinancialAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinancialAPI.Services
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _db;

        public CardService(ApplicationDbContext db)
        {
            _db = db;
        }

        private ulong? GetUserId(ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ulong.TryParse(sub, out var id)) return id;
            return null;
        }

        public async Task<IEnumerable<CardDto>> ListAsync(ClaimsPrincipal user)
        {
            var userId = GetUserId(user);
            if (userId == null) return Enumerable.Empty<CardDto>();

            var items = await _db.CreditCards.Where(c => c.UserId == userId.Value && c.Active).ToListAsync();
            return items.Select(c => new CardDto(c.Id, c.AccountId, c.Name, c.LastFourDigits, c.ClosingDay, c.DueDay));
        }

        public async Task<CardDto?> GetAsync(ClaimsPrincipal user, ulong id)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var card = await _db.CreditCards.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);
            if (card == null) return null;
            return new CardDto(card.Id, card.AccountId, card.Name, card.LastFourDigits, card.ClosingDay, card.DueDay);
        }

        public async Task<CardDto?> CreateAsync(ClaimsPrincipal user, CreateCardDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == dto.AccountId && a.UserId == userId.Value);
            if (account == null) return null;

            var card = new CreditCard
            {
                UserId = userId.Value,
                AccountId = dto.AccountId,
                Name = dto.Name,
                LastFourDigits = dto.LastFourDigits,
                ClosingDay = dto.ClosingDay,
                DueDay = dto.DueDay,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.CreditCards.Add(card);
            await _db.SaveChangesAsync();
            return new CardDto(card.Id, card.AccountId, card.Name, card.LastFourDigits, card.ClosingDay, card.DueDay);
        }

        public async Task<bool> UpdateAsync(ClaimsPrincipal user, ulong id, UpdateCardDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return false;

            var card = await _db.CreditCards.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);
            if (card == null) return false;

            if (!string.IsNullOrEmpty(dto.Name)) card.Name = dto.Name;
            if (dto.ClosingDay.HasValue) card.ClosingDay = dto.ClosingDay;
            if (dto.DueDay.HasValue) card.DueDay = dto.DueDay;
            card.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDueDayAsync(ClaimsPrincipal user, ulong id, byte? newDueDay)
        {
            var userId = GetUserId(user);
            if (userId == null) return false;

            var card = await _db.CreditCards.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);
            if (card == null) return false;

            card.DueDay = newDueDay;
            card.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            // TODO: Trigger bill recalculation in BillService
            return true;
        }
    }
}
