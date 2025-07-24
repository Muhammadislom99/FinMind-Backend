using FinMind.Application.Contract.Categories.Responses;
using FinMind.Application.Contract.Enums;

namespace FinMind.Application.Contract.Categories.Queries;

public record GetCategoriesQuery(CategoryType Type) : IRequest<List<CategoryResponse>>;