using System.ComponentModel;

namespace FinancialAPI.Dtos
{
    public record ContextDto(
        [property: Description("Identificador do contexto"), DefaultValue(2UL)] ulong Id,
        [property: Description("Nome do contexto"), DefaultValue("Família")] string Name);

    public record CreateContextDto(
        [property: Description("Nome do novo contexto"), DefaultValue("Trabalho")] string Name);
}
