using FinancialAPI.Dtos;

namespace FinancialAPI.Services
{
    public interface IContextService
    {
        Task<IEnumerable<ContextDto>> ListAsync(System.Security.Claims.ClaimsPrincipal user);
        Task<ContextDto?> GetAsync(System.Security.Claims.ClaimsPrincipal user, ulong id);
        Task<ContextDto?> CreateAsync(System.Security.Claims.ClaimsPrincipal user, CreateContextDto dto);
        Task<bool> UpdateAsync(System.Security.Claims.ClaimsPrincipal user, ulong id, CreateContextDto dto);
    }
}
