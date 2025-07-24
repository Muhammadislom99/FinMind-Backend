using FinMind.Application.Contract.Enums;

namespace FinMind.Application.Contract.Accounts.Responses;

public record AccountResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public AccountType Type { get; init; }
    public string Currency { get; init; }
    public decimal Balance { get; init; }
}