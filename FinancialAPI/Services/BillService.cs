using FinancialAPI.Data;
using FinancialAPI.Dtos;
using FinancialAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinancialAPI.Services
{
    public class BillService : IBillService
    {
        private readonly ApplicationDbContext _db;

        public BillService(ApplicationDbContext db)
        {
            _db = db;
        }

        private ulong? GetUserId(ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ulong.TryParse(sub, out var id)) return id;
            return null;
        }

        public async Task<IEnumerable<BillDto>> ListByCardAsync(ClaimsPrincipal user, ulong cardId)
        {
            var userId = GetUserId(user);
            if (userId == null) return Enumerable.Empty<BillDto>();

            var card = await _db.CreditCards.FirstOrDefaultAsync(c => c.Id == cardId && c.UserId == userId.Value);
            if (card == null) return Enumerable.Empty<BillDto>();

            var bills = await _db.CreditCardBills.Where(b => b.CreditCardId == cardId && b.UserId == userId.Value).OrderByDescending(b => b.ReferenceYear).ThenByDescending(b => b.ReferenceMonth).ToListAsync();
            return bills.Select(b => new BillDto(b.Id, b.CreditCardId, b.ReferenceYear, b.ReferenceMonth, b.ClosingDate, b.DueDate, b.Status, b.TotalAmount, b.PaidAmount));
        }

        public async Task<BillDto?> GetAsync(ClaimsPrincipal user, ulong id)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var bill = await _db.CreditCardBills.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId.Value);
            if (bill == null) return null;
            return new BillDto(bill.Id, bill.CreditCardId, bill.ReferenceYear, bill.ReferenceMonth, bill.ClosingDate, bill.DueDate, bill.Status, bill.TotalAmount, bill.PaidAmount);
        }

        public async Task<bool> RecalculateAsync(ClaimsPrincipal user, ulong cardId)
        {
            var userId = GetUserId(user);
            if (userId == null) return false;

            var card = await _db.CreditCards.FirstOrDefaultAsync(c => c.Id == cardId && c.UserId == userId.Value);
            if (card == null || !card.DueDay.HasValue) return false;

            // Get existing bills
            var existingBills = await _db.CreditCardBills.Where(b => b.CreditCardId == cardId && b.UserId == userId.Value).ToListAsync();

            // For each bill, recalculate due date based on new due_day
            // Preserve existing transactions and installments by not deleting them
            foreach (var bill in existingBills)
            {
                // Calculate new due date: same month/year as reference, adjusted to due_day
                var newDueDate = new DateTime((int)bill.ReferenceYear, (int)bill.ReferenceMonth, 1)
                    .AddMonths(1)
                    .AddDays(-1); // Last day of the month

                if (card.DueDay.Value <= DateTime.DaysInMonth((int)bill.ReferenceYear, (int)bill.ReferenceMonth))
                {
                    newDueDate = new DateTime((int)bill.ReferenceYear, (int)bill.ReferenceMonth, card.DueDay.Value);
                    // Move to next month if due day is in the month
                    newDueDate = newDueDate.AddMonths(1);
                }

                bill.DueDate = newDueDate;
                bill.UpdatedAt = DateTime.UtcNow;
            }

            // Generate future bills for the next 3 months if they don't exist
            var now = DateTime.UtcNow;
            for (int i = 0; i < 3; i++)
            {
                var futureMonth = now.AddMonths(i);
                var futureYear = (ushort)futureMonth.Year;
                var futureMonthNum = (byte)futureMonth.Month;

                var existingBill = existingBills.FirstOrDefault(b => b.ReferenceYear == futureYear && b.ReferenceMonth == futureMonthNum);
                if (existingBill == null)
                {
                    var daysInMonth = DateTime.DaysInMonth((int)futureYear, futureMonthNum);
                    var dueDay = card.DueDay.HasValue && card.DueDay.Value <= daysInMonth ? card.DueDay.Value : (byte)daysInMonth;

                    var newBill = new CreditCardBill
                    {
                        UserId = userId.Value,
                        CreditCardId = cardId,
                        ReferenceYear = futureYear,
                        ReferenceMonth = futureMonthNum,
                        ClosingDate = null,
                        DueDate = new DateTime((int)futureYear, futureMonthNum, dueDay).AddMonths(1),
                        Status = "OPEN",
                        TotalAmount = 0m,
                        PaidAmount = 0m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _db.CreditCardBills.Add(newBill);
                }
            }

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
