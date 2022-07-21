using Catalog.Services.Dto;

namespace Catalog.Services;

public interface IItemService
{
    Task<ItemDto> GetItemAsync(Guid itemId);
    Task<PagedItems> GetPageItemsAsync(int pageSize, int pageNumber, Guid? categoryId);
    Task<ItemDto> AddAsync(CreateItemDto item);
    Task UpdateAsync(Guid itemId, UpdateItemDto itemDto);
    Task RemoveAsync(Guid itemId);
}
