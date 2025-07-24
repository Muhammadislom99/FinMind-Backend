using FinMind.Domain.Enums;

namespace FinMind.Domain.Entities;

public class Goal : BaseEntity
{
    public decimal TargetAmount { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime TargetDate { get; set; }
    
    public GoalStatus Status { get; set; } = GoalStatus.Active;
    public GoalType Type { get; set; }
    
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
}