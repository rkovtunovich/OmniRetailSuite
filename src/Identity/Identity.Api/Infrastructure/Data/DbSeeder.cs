using Identity.Api.Infrastructure.Repositories;
using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Npgsql;
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
            await AppIdentityDbContextSeed.SeedAsync(identityContext, userManager, roleManager, preferences);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }

   
}
