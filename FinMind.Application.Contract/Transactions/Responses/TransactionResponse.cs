using FinMind.Application.Contract.Enums;

namespace FinMind.Application.Contract.Transactions.Responses;

public record TransactionResponse
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public DateTime DateTime { get; init; }
    public TransactionType Type { get; init; }
    public string Title { get; set; }
    public string SubTitle {get; set; }
    public string? Description { get; init; }
    public List<string> Tags { get; set; }
}
