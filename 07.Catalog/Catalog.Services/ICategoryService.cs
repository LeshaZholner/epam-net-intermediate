using Catalog.Services.Dto;

namespace Catalog.Services;

public interface ICategoryService
{
    Task<CategoryDto> GetCategoryAsync(Guid categoryId);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto> AddAsync(CreateCategoryDto category);
    Task UpdateAsync(Guid categoryId, UpdateCategoryDto category);
    Task RemoveAsync(Guid categoryId);
}
