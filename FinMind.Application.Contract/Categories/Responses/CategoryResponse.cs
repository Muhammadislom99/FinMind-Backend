namespace FinMind.Application.Contract.Categories.Responses;

public record CategoryResponse
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal? Budget { get; init; }
    public string? Icon { get; init; }
    public string? Color { get; init; }
    public bool IsActive { get; init; }
    public List<CategoryResponse> SubCategories { get; init; }
}