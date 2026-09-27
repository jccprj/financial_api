using FinancialAPI.Dtos;

namespace FinancialAPI.Services
{
    public interface IMerchantService
    {
        Task<IEnumerable<MerchantDto>> ListAsync(System.Security.Claims.ClaimsPrincipal user);
        Task<MerchantDto?> GetAsync(System.Security.Claims.ClaimsPrincipal user, ulong id);
        Task<bool> UpdateClassificationAsync(System.Security.Claims.ClaimsPrincipal user, ulong id, MerchantDto dto);
    }
}
