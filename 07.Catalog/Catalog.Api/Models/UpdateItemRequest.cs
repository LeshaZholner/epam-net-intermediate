namespace Catalog.Api.Models;

public class UpdateItemRequest
{
    public string? Name { get; set; }
    public Guid? CategoryId { get; set; }
}
