using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Models;

public class UpdateCategoryRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
