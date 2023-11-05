using Microsoft.AspNetCore.Identity;

namespace Identity.Api.Models;

public class ApplicationUser : IdentityUser
{
    public DateTimeOffset CreatedAt { get; set; }
}
