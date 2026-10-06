using System.ComponentModel;

namespace FinancialAPI.Dtos
{
    public record CategoryDto(
        [property: Description("Identificador da categoria"), DefaultValue(1UL)] ulong Id,
        [property: Description("Nome da categoria"), DefaultValue("Alimentação")] string Name);

    public record CreateCategoryDto(
        [property: Description("Nome da nova categoria"), DefaultValue("Saúde")] string Name);

    public record SubcategoryDto(
        [property: Description("Identificador da subcategoria"), DefaultValue(11UL)] ulong Id,
        [property: Description("Identificador da categoria pai"), DefaultValue(1UL)] ulong CategoryId,
        [property: Description("Nome da subcategoria"), DefaultValue("Restaurante")] string Name);

    public record CreateSubcategoryDto(
        [property: Description("Nome da nova subcategoria"), DefaultValue("Lanche")] string Name);
}
