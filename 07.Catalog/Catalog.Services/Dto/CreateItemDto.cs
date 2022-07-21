namespace Catalog.Services.Dto;

public class CreateItemDto
{
    public string Name { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
}