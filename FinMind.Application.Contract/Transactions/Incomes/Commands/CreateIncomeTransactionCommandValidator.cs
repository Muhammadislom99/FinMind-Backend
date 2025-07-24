namespace FinMind.Application.Contract.Transactions.Incomes.Commands;

public class CreateIncomeTransactionCommandValidator : AbstractValidator<CreateIncomeTransactionCommand>
{
    public CreateIncomeTransactionCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}