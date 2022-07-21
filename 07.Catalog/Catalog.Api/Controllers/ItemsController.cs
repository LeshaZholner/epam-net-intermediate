using Catalog.Api.Models;
using Catalog.Services;
using Catalog.Services.Dto;
using Catalog.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItem(Guid itemId)
        {
            try
            {
                var item = await _itemService.GetItemAsync(itemId);
                return Ok(item);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPageItems(int pageSize, int pageNumber, Guid? categoryId)
        {
            var items = await _itemService.GetPageItemsAsync(pageSize, pageNumber, categoryId);

            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(CreateItemRequest item)
        {
            try
            {
                var itemDto = new CreateItemDto
                {
                    Name = item.Name,
                    CategoryId = item.CategoryId
                };

                var newItem = await _itemService.AddAsync(itemDto);

                return Created(new Uri($"api/items/{newItem.Id}", UriKind.Relative), newItem);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem(Guid itemId, UpdateItemRequest item)
        {
            try
            {
                var itemDto = new UpdateItemDto
                {
                    Name = item.Name,
                    CategoryId = item.CategoryId
                };

                await _itemService.UpdateAsync(itemId, itemDto);

                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            try
            {
                await _itemService.RemoveAsync(itemId);

                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
