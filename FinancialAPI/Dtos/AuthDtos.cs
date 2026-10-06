using System.ComponentModel;

namespace FinancialAPI.Dtos
{
    public record LoginRequest(
        [property: Description("E-mail do usuário"), DefaultValue("usuario@dominio.com")] string Email,
        [property: Description("Senha do usuário (opcional)")] string? Password,
        [property: Description("Token de API alternativo para autenticação (opcional)")] string? ApiToken);

    public record LoginResponse(
        [property: Description("JWT de autenticação no formato Bearer"), DefaultValue("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")] string Token,
        [property: Description("Tempo de expiração em segundos"), DefaultValue(3600L)] long ExpiresIn);

    public record CurrentUserDto(
        [property: Description("Identificador do usuário"), DefaultValue(1L)] long Id,
        [property: Description("Nome do usuário"), DefaultValue("João")] string Name,
        [property: Description("E-mail do usuário"), DefaultValue("usuario@dominio.com")] string Email);
}
