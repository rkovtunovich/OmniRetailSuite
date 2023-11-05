using Identity.Api.Models;
using Identity.Api.Services.Abstractions;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[Route("api/v1/userpreferences")]
public class UserPreferenceController : Controller
{
    private readonly IUserPreferenceService _userPreferenceService;

    public UserPreferenceController(IUserPreferenceService userPreferenceService)
    {
        _userPreferenceService = userPreferenceService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPreferencesAsync(string userId)
    {
        var preferences = await _userPreferenceService.GetPreferencesAsync(userId);

        return Ok(preferences);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdatePreferencesAsync([FromBody] Settings settings, string userId)
    {
        await _userPreferenceService.UpdatePreferencesAsync(userId, settings);

        return Ok();
    }
}
