using FinancialAPI.Dtos;

namespace FinancialAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<CurrentUserDto?> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user);
    }
}
