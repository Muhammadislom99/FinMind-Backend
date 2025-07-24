namespace FinMind.Application.Contract.Transactions.Expenses.Commands;

public class CreateExpenseTransactionCommandValidator : AbstractValidator<CreateExpenseTransactionCommand>
{
    public CreateExpenseTransactionCommandValidator()
    {
        RuleFor(g => g.AccountId).NotEmpty();
        RuleFor(g => g.CategoryId).NotEmpty();
    }
}