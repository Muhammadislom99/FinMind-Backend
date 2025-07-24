using FinMind.Application.Contract.Categories.Queries;
using FinMind.Application.Contract.Categories.Responses;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Categories.Queries;

public class GetCategoriesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private CategoryType _categoryType;

    public async Task<List<CategoryResponse>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        _categoryType = (CategoryType)request.Type;
        var list = await dbContext.Categories.Include(c => c.Account)
            .Where(c => c.Account.UserId == CurrentUser.Id && c.IsActive &&
                        c.Type == _categoryType
                        && c.ParentCategoryId == null)
            .ToListAsync(cancellationToken);

        foreach (var item in list)
        {
            item.Categories = await GetSubCategories(item);
        }

        return mapper.Map<List<CategoryResponse>>(list.ToList());
    }

    private async Task<List<Category>> GetSubCategories(Category category)
    {
        category.Categories = await dbContext.Categories.Include(c => c.Account)
            .Where(c => c.ParentCategoryId == category.Id && c.IsActive && c.Type == _categoryType)
            .ToListAsync();
        foreach (var subCategory in category.Categories)
        {
            subCategory.Categories = await GetSubCategories(subCategory);
        }

        return category.Categories.ToList();
    }
}