namespace FinMind.Application.Contract.Loans.Commands;

public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty()
            .MinimumLength(3);
        RuleFor(x => x.Amount)
            .Must(a => a > 0);
        RuleFor(x => x)
            .Must(x => x.MonthlyPayment > 0);

        RuleFor(x => x)
            .Must(x => x.PrincipalAmount >= x.Amount)
            .WithMessage("PrincipalAmount must be greater than or equal to Amount");

        RuleFor(x => x)
            .Must(x => x.InterestRate >= 0);

        RuleFor(x => x)
            .Must(x => x.EndDate >= x.StartDate);
    }
}