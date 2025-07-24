namespace FinMind.Application.Contract.Transactions.Transfer.Commands;

public class CreateTransferTransactionCommandValidator : AbstractValidator<CreateTransferTransactionCommand>
{
    public CreateTransferTransactionCommandValidator()
    {
        RuleFor(x => x.FromAccountId).NotEmpty();
        RuleFor(x => x.ToAccountId).NotEmpty();
    }
}