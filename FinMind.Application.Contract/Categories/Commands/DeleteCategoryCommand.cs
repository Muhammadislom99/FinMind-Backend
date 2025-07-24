namespace FinMind.Application.Contract.Categories.Commands;

public record DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; init; }
}