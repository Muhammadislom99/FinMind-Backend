using FinMind.Domain.Enums;

namespace FinMind.Domain.Entities;

public class Category : BaseEntity
{
    public CategoryType Type { get; set; }
    public decimal? BudgetAmount { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public bool IsSystem { get; set; } = false;
    public bool IsActive { get; set; } = true;
    
    public Guid? ParentCategoryId { get; set; }
    
    public virtual Category ParentCategory { get; set; }
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
   
}