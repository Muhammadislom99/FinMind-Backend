using FinMind.Domain.Enums;

namespace FinMind.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }= DateTime.UtcNow;
    public string? Note { get; set; }
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public TransactionType Type { get; set; }
    public virtual ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
}