namespace FinMind.Application.Contract.Transactions.Tags.Commands;

public class AddTagsTransactionCommandValidator : AbstractValidator<AddTagsTransactionCommand>
{
    public AddTagsTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionId).NotEmpty();
        RuleFor(x => x.Tags)
            .NotEmpty()
            .NotNull()
            .Must(tag => !tag.Any(string.IsNullOrWhiteSpace));
    }
}