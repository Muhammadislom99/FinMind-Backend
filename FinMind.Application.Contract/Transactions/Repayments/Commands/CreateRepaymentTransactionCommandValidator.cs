namespace FinMind.Application.Contract.Transactions.Repayments.Commands;

public class CreateRepaymentTransactionCommandValidator : AbstractValidator<CreateRepaymentTransactionCommand>
{
    public CreateRepaymentTransactionCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().NotNull();
        RuleFor(x => x.LoanId).NotEmpty().NotNull();
    }
}