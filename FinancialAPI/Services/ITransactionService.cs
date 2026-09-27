using FinancialAPI.Dtos;

namespace FinancialAPI.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> ListAsync(System.Security.Claims.ClaimsPrincipal user, TransactionFilterParams? filters = null);
        Task<TransactionDto?> GetAsync(System.Security.Claims.ClaimsPrincipal user, ulong id);
        Task<IEnumerable<TransactionDto>> ListPendingClassificationsAsync(System.Security.Claims.ClaimsPrincipal user);
        Task<bool> ClassifyAsync(System.Security.Claims.ClaimsPrincipal user, ulong id, ClassifyTransactionDto dto);
    }
}
