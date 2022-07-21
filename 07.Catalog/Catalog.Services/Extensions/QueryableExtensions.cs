using Microsoft.EntityFrameworkCore;

namespace Catalog.Services.Extensions;

public static class QueryableExtensions
{
    public static PagedList<T> ToPageList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        return new PagedList<T>(source, pageNumber, pageSize);
    }

    public static async Task<PagedList<T>> ToPageListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        var totalCount = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new PagedList<T>(items, pageNumber, pageSize, totalCount);
    }
}

public class PagedList<T> : List<T>
{
    public int PageSize { get; private set; }
    public int PageNumber { get; private set; }
    public int PageCount { get; private set; }
    public int TotalCount { get; private set; }

    public bool HasNextPage => PageNumber < PageCount;

    public PagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = source.Count();
        PageCount = (int)Math.Ceiling((double)TotalCount / pageSize);

        AddRange(source.Skip(pageNumber * pageSize).Take(pageSize));
    }

    public PagedList(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        PageCount = (int)Math.Ceiling((double)TotalCount / pageSize);
        
        AddRange(data);
    }
}