namespace FinMind.Application.Contract.Accounts.Commands;

public record SetCheckingAccountCommand : IRequest<bool>
{
    public Guid AccountId { get; set; }
}