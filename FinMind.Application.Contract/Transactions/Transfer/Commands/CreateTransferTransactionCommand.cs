namespace FinMind.Application.Contract.Transactions.Transfer.Commands;

public record CreateTransferTransactionCommand : CreateTransactionCommand
{
    public Guid FromAccountId { get; init; }
    public Guid ToAccountId { get; init; }
}