namespace FinMind.Application.Contract.Transactions.Tags.Responses;

public record TransactionTagsResponse
{
    public Guid TransactionId { get; init; }
    public List<String> Tags { get; init; }
}