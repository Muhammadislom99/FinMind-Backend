namespace FinMind.Application.Contract.Loans.Commands;

public class DeleteLoanCommandValidator : AbstractValidator<DeleteLoanCommand>
{
    public DeleteLoanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}