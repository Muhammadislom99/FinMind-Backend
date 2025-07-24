using FinMind.Application.Contract.Enums;

namespace FinMind.Application.Contract.Categories.Commands;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Type)
            .Must(type => Enum.IsDefined(typeof(CategoryType), type));
    }
}