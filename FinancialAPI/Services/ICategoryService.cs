using FinancialAPI.Dtos;

namespace FinancialAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> ListAsync(System.Security.Claims.ClaimsPrincipal user);
        Task<CategoryDto?> GetAsync(System.Security.Claims.ClaimsPrincipal user, ulong id);
        Task<CategoryDto?> CreateAsync(System.Security.Claims.ClaimsPrincipal user, CreateCategoryDto dto);
        Task<bool> UpdateAsync(System.Security.Claims.ClaimsPrincipal user, ulong id, CreateCategoryDto dto);
        Task<IEnumerable<SubcategoryDto>> ListSubcategoriesAsync(System.Security.Claims.ClaimsPrincipal user, ulong categoryId);
        Task<SubcategoryDto?> CreateSubcategoryAsync(System.Security.Claims.ClaimsPrincipal user, ulong categoryId, CreateSubcategoryDto dto);
        Task<bool> UpdateSubcategoryAsync(System.Security.Claims.ClaimsPrincipal user, ulong id, CreateSubcategoryDto dto);
    }
}
