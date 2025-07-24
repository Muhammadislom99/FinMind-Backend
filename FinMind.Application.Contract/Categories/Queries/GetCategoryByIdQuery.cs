using FinMind.Application.Contract.Categories.Responses;

namespace FinMind.Application.Contract.Categories.Queries;

public record GetCategoryByIdQuery : IRequest<CategoryResponse>
{
    public Guid Id { get; init; }
}