#nullable enable
using FinMind.Application.Contract.Enums;

namespace FinMind.Application.Contract.Categories.Commands;

public record CreateCategoryCommand : IRequest<bool>
{
   
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

    public CategoryType Type { get; init; }
}