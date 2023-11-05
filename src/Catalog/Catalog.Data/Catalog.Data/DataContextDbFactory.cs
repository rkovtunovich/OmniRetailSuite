using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Catalog.Data;

public class DataContextDbFactory: IDesignTimeDbContextFactory<CatalogContext>
{
    public CatalogContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("CatalogConnection"));
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new CatalogContext(optionsBuilder.Options);
    }
}
