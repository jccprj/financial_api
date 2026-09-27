using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using FinancialAPI.Data;
using FinancialAPI.Dtos;
using FinancialAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly string _key;
        private readonly string _issuer;

        public AuthService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _key = config.GetSection("Jwt").GetValue<string>("Key") ?? "replace_this_with_a_real_secret";
            _issuer = config.GetSection("Jwt").GetValue<string>("Issuer") ?? "financial_api";
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email)) return null;

            var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return null;

            // Accept if api token matches or password hash is not set (simple fallback for seed)
            if (!string.IsNullOrEmpty(request.ApiToken) && request.ApiToken == user.ApiToken)
            {
                return GenerateToken(user);
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return GenerateToken(user);
            }

            // Password validation omitted - expect hashed comparison in real app
            return null;
        }

        public Task<CurrentUserDto?> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            if (user?.Identity == null || !user.Identity.IsAuthenticated) return Task.FromResult<CurrentUserDto?>(null);

            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!long.TryParse(sub, out var id)) return Task.FromResult<CurrentUserDto?>(null);

            var appUser = _db.AppUsers.Find((ulong)id);
            if (appUser == null) return Task.FromResult<CurrentUserDto?>(null);

            return Task.FromResult<CurrentUserDto?>(new CurrentUserDto((long)appUser.Id, appUser.Name, appUser.Email));
        }

        private LoginResponse GenerateToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new LoginResponse(jwt, (long)TimeSpan.FromHours(8).TotalSeconds);
        }
    }
}
