namespace FinMind.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public abstract class BaseEntityWithNameDescription : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}