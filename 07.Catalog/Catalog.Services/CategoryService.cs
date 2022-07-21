using Catalog.Data;
using Catalog.Data.Models;
using Catalog.Services.Dto;
using Catalog.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Services;

public class CategoryService : ICategoryService
{
    private readonly CatalogDbContext _context;

    public CategoryService(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto> GetCategoryAsync(Guid categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);

        EntityNotFoundException.ThrowIfNull(category, $"A category having id {categoryId} could not be found.");
        
        return new CategoryDto { Id = category.Id, Name = category.Name };
    }

    public async Task<CategoryDto> AddAsync(CreateCategoryDto category)
    {
        var newCategory = _context.Categories.Add(new Category { Name = category.Name }).Entity;
        await _context.SaveChangesAsync();

        return new CategoryDto { Id = newCategory.Id, Name = newCategory.Name };
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _context.Categories.Select(c => new CategoryDto 
            { 
                Id = c.Id, 
                Name = c.Name
            }).ToListAsync();

        return categories;
    }

    public async Task RemoveAsync(Guid categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        
        EntityNotFoundException.ThrowIfNull(category, $"A category having id {categoryId} could not be found.");

        _context.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid categoryId, UpdateCategoryDto category)
    {
        var updatedCategory = await _context.Categories.FindAsync(categoryId);

        EntityNotFoundException.ThrowIfNull(updatedCategory, $"A category having id {categoryId} could not be found.");

        updatedCategory.Name = category.Name;

        _context.Categories.Update(updatedCategory);
        await _context.SaveChangesAsync();
    }
}
