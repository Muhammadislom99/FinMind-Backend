using FinMind.Application.Contract.Transactions.Responses;

namespace FinMind.Application.Contract.Transactions.Queries;

public record GetTransactionsByAccountQuery : IRequest<List<TransactionResponse>>
{
    public Guid AccountId { get; init; }
}