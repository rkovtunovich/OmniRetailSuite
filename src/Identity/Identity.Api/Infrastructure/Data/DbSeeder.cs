using Identity.Api.Infrastructure.Repositories;
using Identity.Api.Models;
using Infrastructure.DataManagement.Postgres.Configuration.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Api.Infrastructure.Data;

public class DbSeeder
{
    public async static Task SeedDatabase(WebApplication app)
    {
        app.Logger.LogInformation("Seeding Database...");

        using var scope = app.Services.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        try
        {
            var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var identityContext = scopedProvider.GetRequiredService<ApplicationDbContext>();
            var preferences = scopedProvider.GetRequiredService<IUserPreferenceRepository>();
            var dbManager = scopedProvider.GetRequiredService<IDbManager>();

            var dbOptions = scopedProvider.GetRequiredService<IOptions<DbSettings>>();

            await dbManager.EnsureDatabaseExists(dbOptions.Value.Database);

            await AppIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager, preferences);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }  
}
