namespace FinMind.Domain.Entities;


public class TransactionTag : BaseEntity // Связь транзакций с тегами (многие ко многим)
{
    public Guid TransactionId { get; set; } 
    public virtual Transaction Transaction { get; set; }
    
    public Guid TagId { get; set; }
    public virtual Tag Tag { get; set; }
}