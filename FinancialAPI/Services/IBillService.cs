using FinancialAPI.Dtos;

namespace FinancialAPI.Services
{
    public interface IBillService
    {
        Task<IEnumerable<BillDto>> ListByCardAsync(System.Security.Claims.ClaimsPrincipal user, ulong cardId);
        Task<BillDto?> GetAsync(System.Security.Claims.ClaimsPrincipal user, ulong id);
        Task<bool> RecalculateAsync(System.Security.Claims.ClaimsPrincipal user, ulong cardId);
    }
}
