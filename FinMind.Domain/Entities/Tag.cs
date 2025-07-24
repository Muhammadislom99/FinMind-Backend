namespace FinMind.Domain.Entities;

public class Tag : BaseEntityWithNameDescription
{
    public string? Color { get; set; }
    public virtual ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
}