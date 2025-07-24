namespace FinMind.Application.Contract.Loans.Commands;

public record DeleteLoanCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}