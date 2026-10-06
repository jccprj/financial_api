using System.ComponentModel;

namespace FinancialAPI.Dtos
{
    public record MerchantDto(
        [property: Description("Identificador do estabelecimento"), DefaultValue(10UL)] ulong Id,
        [property: Description("Nome do estabelecimento"), DefaultValue("Amazon")] string Name,
        [property: Description("Indica se requer classificação manual"), DefaultValue(true)] bool RequiresClassification);
}
