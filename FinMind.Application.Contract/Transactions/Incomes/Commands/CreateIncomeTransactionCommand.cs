namespace FinMind.Application.Contract.Transactions.Incomes.Commands;

public record CreateIncomeTransactionCommand : CreateTransactionCommand
{
    public Guid CategoryId { get; set; }
    public Guid AccountId { get; set; }
}