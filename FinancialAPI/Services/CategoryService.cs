using FinancialAPI.Data;
using FinancialAPI.Dtos;
using FinancialAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinancialAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        private ulong? GetUserId(ClaimsPrincipal user)
        {
            var sub = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ulong.TryParse(sub, out var id)) return id;
            return null;
        }

        public async Task<IEnumerable<CategoryDto>> ListAsync(ClaimsPrincipal user)
        {
            var userId = GetUserId(user);
            if (userId == null) return Enumerable.Empty<CategoryDto>();

            var items = await _db.Categories.Where(c => c.UserId == userId.Value && c.Active).ToListAsync();
            return items.Select(c => new CategoryDto(c.Id, c.Name));
        }

        public async Task<CategoryDto?> GetAsync(ClaimsPrincipal user, ulong id)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var item = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);
            if (item == null) return null;
            return new CategoryDto(item.Id, item.Name);
        }

        public async Task<CategoryDto?> CreateAsync(ClaimsPrincipal user, CreateCategoryDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var cat = new Category
            {
                UserId = userId.Value,
                Name = dto.Name,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Categories.Add(cat);
            await _db.SaveChangesAsync();
            return new CategoryDto(cat.Id, cat.Name);
        }

        public async Task<bool> UpdateAsync(ClaimsPrincipal user, ulong id, CreateCategoryDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return false;

            var cat = await _db.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId.Value);
            if (cat == null) return false;

            cat.Name = dto.Name;
            cat.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SubcategoryDto>> ListSubcategoriesAsync(ClaimsPrincipal user, ulong categoryId)
        {
            var userId = GetUserId(user);
            if (userId == null) return Enumerable.Empty<SubcategoryDto>();

            var cat = await _db.Categories.FindAsync(categoryId);
            if (cat == null || cat.UserId != userId.Value) return Enumerable.Empty<SubcategoryDto>();

            var items = await _db.Subcategories.Where(s => s.CategoryId == categoryId && s.Active).ToListAsync();
            return items.Select(s => new SubcategoryDto(s.Id, s.CategoryId, s.Name));
        }

        public async Task<SubcategoryDto?> CreateSubcategoryAsync(ClaimsPrincipal user, ulong categoryId, CreateSubcategoryDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return null;

            var cat = await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId.Value);
            if (cat == null) return null;

            var sub = new Subcategory
            {
                CategoryId = categoryId,
                Name = dto.Name,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Subcategories.Add(sub);
            await _db.SaveChangesAsync();
            return new SubcategoryDto(sub.Id, sub.CategoryId, sub.Name);
        }

        public async Task<bool> UpdateSubcategoryAsync(ClaimsPrincipal user, ulong id, CreateSubcategoryDto dto)
        {
            var userId = GetUserId(user);
            if (userId == null) return false;

            var sub = await _db.Subcategories.FindAsync(id);
            if (sub == null) return false;

            var cat = await _db.Categories.FirstOrDefaultAsync(c => c.Id == sub.CategoryId && c.UserId == userId.Value);
            if (cat == null) return false;

            sub.Name = dto.Name;
            sub.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
