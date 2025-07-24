namespace FinMind.Application.Contract.Accounts.Commands;

public record DeleteAccountCommand() : IRequest<bool>
{
    public Guid Id { get; init; }
}