using FinMind.Application.Contract.Transactions.Tags.Responses;

namespace FinMind.Application.Contract.Transactions.Tags.Commands;

public record AddTagsTransactionCommand : IRequest<TransactionTagsResponse>
{
    public Guid TransactionId { get; init; }
    public List<string> Tags { get; init; } = [];
}