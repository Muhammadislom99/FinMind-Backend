namespace FinMind.Application.Contract.Transactions.Expenses.Commands;

public record CreateExpenseTransactionCommand : CreateTransactionCommand
{
    public Guid AccountId { get; init; }
    public Guid CategoryId { get; init; }
}