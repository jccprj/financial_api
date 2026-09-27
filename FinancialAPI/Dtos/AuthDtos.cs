namespace FinancialAPI.Dtos
{
    public record LoginRequest(string Email, string? Password, string? ApiToken);
    public record LoginResponse(string Token, long ExpiresIn);
    public record CurrentUserDto(long Id, string Name, string Email);
}
