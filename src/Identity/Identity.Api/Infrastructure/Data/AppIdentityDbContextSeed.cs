using Identity.Api.Configuration;
using Identity.Api.Infrastructure.Repositories;
using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Infrastructure.Data;

public class AppIdentityDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext identityDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserPreferenceRepository preferenceRepository)
    {
        if (identityDbContext.Database.IsNpgsql())       
            identityDbContext.Database.Migrate();
        
        await roleManager.CreateAsync(new IdentityRole(Constants.Roles.ADMINISTRATORS));

        await CreateAdminUser(userManager, preferenceRepository);
        await CreateDefaultUser(userManager, preferenceRepository);
    }

    private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager, IUserPreferenceRepository preferenceRepository)
    {
        var adminUserName = "admin";
        var adminUser = new ApplicationUser
        {
            UserName = adminUserName,
            Email = "admin@microsoft.com",
            CreatedAt = DateTimeOffset.UtcNow
        };
        await userManager.CreateAsync(adminUser, AuthorizationConstants.DEFAULT_PASSWORD);
        adminUser = await userManager.FindByNameAsync(adminUserName) ?? throw new Exception("Seeding DB error");

        await userManager.AddToRoleAsync(adminUser, Constants.Roles.ADMINISTRATORS);  
        
        await SeedPreferencesAdminUser(preferenceRepository, adminUser.Id);
    }

    private static async Task CreateDefaultUser(UserManager<ApplicationUser> userManager, IUserPreferenceRepository preferenceRepository)
    {
        var defaultUser = new ApplicationUser
        {
            UserName = "demouser",
            Email = "demouser@microsoft.com",
            CreatedAt = DateTimeOffset.UtcNow
        };
        await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);

        defaultUser = await userManager.FindByNameAsync(defaultUser.UserName) ?? throw new Exception("Seeding DB error");

        await SeedPreferencesDefaultUser(preferenceRepository, defaultUser.Id);       
    }

    private static async Task SeedPreferencesAdminUser(IUserPreferenceRepository userPreference, string userId)
    {
        if(await userPreference.GetPreferencesAsync(userId) != null) 
            return;

        var adminUserPreference = new UserPreference
        {
            UserId = userId,
            Settings = new Settings
            {
                Language = "en",
                Theme = "dark"
            },
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await userPreference.UpdatePreferencesAsync(adminUserPreference);
    }

    private static async Task SeedPreferencesDefaultUser(IUserPreferenceRepository userPreference, string userId)
    {
        if (await userPreference.GetPreferencesAsync(userId) != null)
            return;

        var defaultUserPreference = new UserPreference
        {
            UserId = userId,
            Settings = new Settings
            {
                Language = "en",
                Theme = "light"
            },
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await userPreference.UpdatePreferencesAsync(defaultUserPreference);
    }
}
