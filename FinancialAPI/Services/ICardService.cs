using FinancialAPI.Dtos;

namespace FinancialAPI.Services
{
    public interface ICardService
    {
        Task<IEnumerable<CardDto>> ListAsync(System.Security.Claims.ClaimsPrincipal user);
        Task<CardDto?> GetAsync(System.Security.Claims.ClaimsPrincipal user, ulong id);
        Task<CardDto?> CreateAsync(System.Security.Claims.ClaimsPrincipal user, CreateCardDto dto);
        Task<bool> UpdateAsync(System.Security.Claims.ClaimsPrincipal user, ulong id, UpdateCardDto dto);
        Task<bool> UpdateDueDayAsync(System.Security.Claims.ClaimsPrincipal user, ulong id, byte? newDueDay);
    }
}
