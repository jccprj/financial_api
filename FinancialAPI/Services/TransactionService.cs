using FinancialAPI.Data;
using FinancialAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinancialAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _db;

        public TransactionService(ApplicationDbContext db)
        {
            _db = db;
        }

        private ulong? GetUserId(ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ulong.TryParse(sub, out var id)) return id;
            return null;
        }

        public async Task<IEnumerable<TransactionDto>> ListAsync(ClaimsPrincipal user, TransactionFilterParams? filters = null)
        {
            var userId = GetUserId(user);
            if (userId == null) return Enumerable.Empty<TransactionDto>();

            var q = _db.FinancialTransactions.Where(t => t.UserId == userId.Value).AsQueryable();

            if (filters != null)
            {
                if (filters.DateFrom.HasValue) q = q.Where(t => t.FinancialTransactionDate >= filters.DateFrom.Value);
                if (filters.DateTo.HasValue) q = q.Where(t => t.FinancialTransactionDate <= filters.DateTo.Value);
                if (filters.MinAmount.HasValue) q = q.Where(t => t.Amount >= filters.MinAmount.Value);
                if (filters.MaxAmount.HasValue) q = q.Where(t => t.Amount <= filters.MaxAmount.Value);
                if (filters.MerchantId.HasValue) q = q.Where(t => t.MerchantId == filters.MerchantId.Value);
                if (!string.IsNullOrEmpty(filters.Status)) q = q.Where(t => t.ClassificationStatus == filters.Status);
            }

            var list = await q.OrderByDescending(t => t.FinancialTransactionDate).Take(100).ToListAsync();

            return list.Select(t => new TransactionDto(t.Id, t.Amount, t.Currency, t.FinancialTransactionDate, t.Description, t.ClassificationStatus, t.CreditCardId, t.MerchantId));
        }

        public async Task<TransactionDto?> GetAsync(ClaimsPrincipal user, ulong id)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var t = await _db.FinancialTransactions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId.Value);
            if (t == null) return null;
            return new TransactionDto(t.Id, t.Amount, t.Currency, t.FinancialTransactionDate, t.Description, t.ClassificationStatus, t.CreditCardId, t.MerchantId);
        }

        public async Task<IEnumerable<TransactionDto>> ListPendingClassificationsAsync(ClaimsPrincipal user)
        {
            var userId = GetUserId(user);
            if (userId == null) return Enumerable.Empty<TransactionDto>();

            var list = await _db.FinancialTransactions.Where(t => t.UserId == userId.Value && t.ClassificationStatus == "PENDING").ToListAsync();
            return list.Select(t => new TransactionDto(t.Id, t.Amount, t.Currency, t.FinancialTransactionDate, t.Description, t.ClassificationStatus, t.CreditCardId, t.MerchantId));
        }

        public async Task<bool> ClassifyAsync(ClaimsPrincipal user, ulong id, ClassifyTransactionDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return false;

            var t = await _db.FinancialTransactions.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId.Value);
            if (t == null) return false;

            if (dto.MerchantId.HasValue) t.MerchantId = dto.MerchantId.Value;
            // category/subcategory/context may require additional tables - keep DB fields update simple by marking classification confirmed
            t.ClassificationStatus = "CONFIRMED";
            t.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
