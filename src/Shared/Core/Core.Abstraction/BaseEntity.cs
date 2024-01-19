namespace Core.Abstraction;

public abstract class BaseEntity
{
    public virtual Guid Id { get; init; }

    public virtual bool IsDeleted { get; set; }

    public virtual DateTimeOffset CreatedAt { get; set; }

    public virtual DateTimeOffset? UpdatedAt { get; set; }
}
