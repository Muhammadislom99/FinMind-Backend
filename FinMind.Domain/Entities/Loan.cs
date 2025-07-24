using FinMind.Domain.Enums;

namespace FinMind.Domain.Entities;

public class Loan : BaseEntity
{
    public LoanType Type { get; set; }
    public decimal Amount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal MonthlyPayment { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Active;
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
}