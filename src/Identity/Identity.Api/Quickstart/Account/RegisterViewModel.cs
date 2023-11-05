
namespace Identity.Api.Quickstart.Account;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string ConfirmPassword { get; set; } = null!;

    public string ReturnUrl { get; set; } = null!;
}
