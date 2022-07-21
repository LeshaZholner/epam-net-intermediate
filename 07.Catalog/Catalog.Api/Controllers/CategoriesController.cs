using Catalog.Api.Models;
using Catalog.Services;
using Catalog.Services.Dto;
using Catalog.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryAsync(id);
                return Ok(category);
            } 
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest category)
        {
            var categoryDto = new CreateCategoryDto { Name = category.Name };
            var newCategory = await _categoryService.AddAsync(categoryDto);

            return Created(new Uri($"api/categories/{newCategory.Id}", UriKind.Relative), newCategory);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, UpdateCategoryRequest category)
        {
            try
            {
                await _categoryService.UpdateAsync(categoryId, new UpdateCategoryDto { Name = category.Name });

                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> RemoveCategory(Guid categoryId)
        {
            try
            {
                await _categoryService.RemoveAsync(categoryId);

                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
