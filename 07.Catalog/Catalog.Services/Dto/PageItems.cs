namespace Catalog.Services.Dto;

public class PagedItems
{
    public List<ItemDto> Items { get; set; } = new();
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalCount { get; set; }
    public int PageCount { get; set; }
}
