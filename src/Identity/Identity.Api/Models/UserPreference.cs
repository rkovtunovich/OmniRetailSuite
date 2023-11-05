using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Api.Models;

public class UserPreference
{
    [Key]
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;

    [Column(TypeName = "jsonb")]
    public Settings Settings { get; set; } = null!;

    public DateTimeOffset UpdatedAt { get; set; }
}
