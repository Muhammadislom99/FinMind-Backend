using FinMind.Application.Contract.Loans.Responses;

namespace FinMind.Application.Contract.Loans.Commands;

public record CreateLoanCommand : IRequest<LoanResponse>
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public decimal Amount { get; init; }
    public decimal PrincipalAmount { get; init; }
    public decimal InterestRate { get; init; }
    public decimal MonthlyPayment { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly EndDate { get; init; }
}