namespace FinancialAPI.Dtos
{
    public record CardDto(ulong Id, ulong AccountId, string? Name, string LastFourDigits, byte? ClosingDay, byte? DueDay);
    public record CreateCardDto(ulong AccountId, string? Name, string LastFourDigits, byte? ClosingDay, byte? DueDay);
    public record UpdateCardDto(string? Name, byte? ClosingDay, byte? DueDay);
}
