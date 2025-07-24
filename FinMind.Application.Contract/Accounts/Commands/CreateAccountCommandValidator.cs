namespace FinMind.Application.Contract.Accounts.Commands;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x=>x.Name)
            .NotEmpty().MaximumLength(25)
            .MinimumLength(3);
        RuleFor(x => x.Type)
            .NotEmpty();
    }
}