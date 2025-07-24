namespace FinMind.Application.Contract.Transactions.Repayments.Commands;

public record CreateRepaymentTransactionCommand : CreateTransactionCommand
{
    public Guid AccountId { get; init; }
    public Guid LoanId { get; init; }
}