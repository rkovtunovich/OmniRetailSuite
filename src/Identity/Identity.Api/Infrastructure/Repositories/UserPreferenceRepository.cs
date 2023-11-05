using Identity.Api.Infrastructure.Data;
using Identity.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Infrastructure.Repositories;

public class UserPreferenceRepository : IUserPreferenceRepository
{
    private readonly ApplicationDbContext _context;

    public UserPreferenceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserPreference?> GetPreferencesAsync(string userId)
    {
        return await _context.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task UpdatePreferencesAsync(UserPreference preferences)
    {
        if (preferences.Id == 0)      
            _context.UserPreferences.Add(preferences);     
        else       
            _context.UserPreferences.Update(preferences);
        
        preferences.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
    }
}

