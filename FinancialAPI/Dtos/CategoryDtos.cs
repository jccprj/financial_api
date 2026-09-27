namespace FinancialAPI.Dtos
{
    public record CategoryDto(ulong Id, string Name);
    public record CreateCategoryDto(string Name);
    public record SubcategoryDto(ulong Id, ulong CategoryId, string Name);
    public record CreateSubcategoryDto(string Name);
}
