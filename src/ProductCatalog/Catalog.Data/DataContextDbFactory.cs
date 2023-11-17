using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductCatalog.Data;

public class DataContextDbFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("CatalogConnection"));
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new ProductDbContext(optionsBuilder.Options);
    }
}
