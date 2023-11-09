using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Retail.Data;

public class DataContextDbFactory : IDesignTimeDbContextFactory<RetailDbContext>
{
    public RetailDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<RetailDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("CatalogConnection"));
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new RetailDbContext(optionsBuilder.Options);
    }
}
