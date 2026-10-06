using System.ComponentModel;

namespace FinancialAPI.Dtos
{
    public record CardDto(
        [property: Description("Identificador do cartão"), DefaultValue(4UL)] ulong Id,
        [property: Description("Identificador da conta"), DefaultValue(3UL)] ulong AccountId,
        [property: Description("Nome amigável do cartão"), DefaultValue("Cartão Principal")] string? Name,
        [property: Description("Últimos 4 dígitos do cartão"), DefaultValue("1234")] string LastFourDigits,
        [property: Description("Dia de fechamento da fatura")] byte? ClosingDay,
        [property: Description("Dia de vencimento da fatura")] byte? DueDay);

    public record CreateCardDto(
        [property: Description("Identificador da conta"), DefaultValue(3UL)] ulong AccountId,
        [property: Description("Nome amigável do cartão"), DefaultValue("Cartão Principal")] string? Name,
        [property: Description("Últimos 4 dígitos do cartão"), DefaultValue("1234")] string LastFourDigits,
        [property: Description("Dia de fechamento da fatura")] byte? ClosingDay,
        [property: Description("Dia de vencimento da fatura")] byte? DueDay);

    public record UpdateCardDto(
        [property: Description("Nome amigável do cartão"), DefaultValue("Cartão Secundário")] string? Name,
        [property: Description("Dia de fechamento da fatura")] byte? ClosingDay,
        [property: Description("Dia de vencimento da fatura")] byte? DueDay);
}
