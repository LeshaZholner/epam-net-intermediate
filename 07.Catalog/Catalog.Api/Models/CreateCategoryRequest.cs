using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Models;

public class CreateCategoryRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
