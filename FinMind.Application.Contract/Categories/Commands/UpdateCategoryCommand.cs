using System.Text.Json.Serialization;
using FinMind.Application.Contract.Categories.Responses;

namespace FinMind.Application.Contract.Categories.Commands;

public record UpdateCategoryCommand : IRequest<CategoryResponse>
{
    [JsonIgnore] public Guid Id { get; set; }

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        init => _name = value?.Trim() ?? string.Empty;
    }

    public string? Description { get; init; }
    public string? Color { get; init; }
    public string? Icon { get; init; }
    public Guid? ParentCategoryId { get; init; }
}