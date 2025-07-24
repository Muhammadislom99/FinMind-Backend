namespace FinMind.Application.Contract.Accounts.Commands;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}