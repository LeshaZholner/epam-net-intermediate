using Catalog.Data;
using Catalog.Data.Models;
using Catalog.Services.Dto;
using Catalog.Services.Exceptions;
using Catalog.Services.Extensions;

namespace Catalog.Services;

public class ItemService : IItemService
{
    private readonly CatalogDbContext _context;

    public ItemService(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<ItemDto> GetItemAsync(Guid itemId)
    {
        var item = await _context.Items.FindAsync(itemId);

        EntityNotFoundException.ThrowIfNull(item, $"An item having id {itemId} could not be found.");

        return new ItemDto { Id = item.Id, Name = item.Name, CategoryId = item.CategoryId };
    }

    public async Task<ItemDto> AddAsync(CreateItemDto item)
    {
        var category = await _context.Categories.FindAsync(item.CategoryId);

        EntityNotFoundException.ThrowIfNull(category, $"A category having id {item.CategoryId} could not be found.");

        var newItem = new Item
        {
            Name = item.Name,
            CategoryId = item.CategoryId
        };

        var createdItem = _context.Add(newItem).Entity;
        await _context.SaveChangesAsync();
        
        return new ItemDto { Id = createdItem.Id, Name = createdItem.Name, CategoryId = createdItem.CategoryId };
    }

    public async Task<PagedItems> GetPageItemsAsync(int pageSize, int pageNumber, Guid? categoryId)
    {
        var query = _context.Items.AsQueryable();

        if (categoryId != null)
        {
            query = query.Where(i => i.CategoryId == categoryId);
        }

        var pageItems = await query.ToPageListAsync(pageNumber, pageSize);

        var pageItemsDto = new PagedItems
        {
            Items = pageItems.Select(i => new ItemDto 
            { 
                Id = i.Id,
                Name = i.Name,
                CategoryId = i.CategoryId
            }).ToList(),
            PageNumber = pageItems.PageNumber,
            PageSize = pageItems.PageSize,
            PageCount = pageItems.PageCount,
            TotalCount = pageItems.TotalCount
        };

        return pageItemsDto;
    }

    public async Task RemoveAsync(Guid itemId)
    {
        var item = await _context.Items.FindAsync(itemId);

        EntityNotFoundException.ThrowIfNull(item, $"An item having id {itemId} could not be found.");

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid itemId, UpdateItemDto itemDto)
    {
        var item = await _context.Items.FindAsync(itemId);

        EntityNotFoundException.ThrowIfNull(item, $"An item having id {itemId} could not be found.");

        if (itemDto.CategoryId != null)
        {
            var category = await _context.Categories.FindAsync(itemDto.CategoryId);

            EntityNotFoundException.ThrowIfNull(category, $"A category having id {itemDto.CategoryId} could not be found.");
        }

        item.Name = itemDto.Name ?? item.Name;
        item.CategoryId = itemDto.CategoryId ?? item.CategoryId;

        _context.Items.Update(item);

        await _context.SaveChangesAsync();
    }
}
