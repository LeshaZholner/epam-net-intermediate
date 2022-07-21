using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Models;

public class CreateItemRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Guid CategoryId { get; set; }
}
