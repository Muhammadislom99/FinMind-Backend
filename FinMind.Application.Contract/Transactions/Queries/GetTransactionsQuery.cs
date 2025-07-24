using FinMind.Application.Contract.Enums;
using FinMind.Application.Contract.Transactions.Responses;

namespace FinMind.Application.Contract.Transactions.Queries;

public record GetTransactionsQuery : IRequest<List<TransactionResponse>>
{
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public TransactionType? Type { get; init; }
    public Guid? AccountId { get; init; }
}

