using Catalog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Services;

public static class CatalogServiceExtensions
{
    public static IServiceCollection AddCatalogService(this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IItemService, ItemService>();

        return services;
    }
}
