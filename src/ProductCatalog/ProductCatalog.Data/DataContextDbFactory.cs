using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Data;

// This class is used by EF Core tools to create migrations
public class DataContextDbFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.db.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("CatalogConnection"));
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new ProductDbContext(optionsBuilder.Options);
    }
}
