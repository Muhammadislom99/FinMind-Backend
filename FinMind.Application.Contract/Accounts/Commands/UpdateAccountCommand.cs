using FinMind.Application.Contract.Accounts.Responses;
using FinMind.Application.Contract.Enums;

namespace FinMind.Application.Contract.Accounts.Commands;

public record UpdateAccountCommand : IRequest<AccountResponse>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public AccountType Type { get; init; }
}