namespace FinancialAPI.Dtos
{
    public record ContextDto(ulong Id, string Name);
    public record CreateContextDto(string Name);
}
