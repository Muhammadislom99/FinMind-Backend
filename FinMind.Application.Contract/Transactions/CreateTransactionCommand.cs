namespace FinMind.Application.Contract.Transactions;

public abstract record CreateTransactionCommand : IRequest<string>
{
    public decimal Amount { get; init; }
    public string? Description { get; init; } = string.Empty;
    public DateTime? DateTime { get; init; }
}

public class CreateIncomeTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateIncomeTransactionCommandValidator()
    {
        RuleFor(x => x.Amount)
            .Must(amount => amount > 0)
            .WithMessage("Amount must be greater than 0");
    }
}