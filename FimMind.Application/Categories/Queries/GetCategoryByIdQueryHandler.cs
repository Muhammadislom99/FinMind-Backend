using FinMind.Application.Contract.Categories.Queries;
using FinMind.Application.Contract.Categories.Responses;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Categories.Queries;

public class GetCategoryByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories.Include(c => c.Account)
            .Include(c => c.Categories)
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.Account.UserId == CurrentUser.Id, cancellationToken);
        if (category == null)
            throw new NotFoundException("Category not found");

        await GetSubCategories(category);

        return mapper.Map<CategoryResponse>(category);
    }

    private async Task<List<Category>> GetSubCategories(Category category)
    {
        category.Categories = await dbContext.Categories.Include(c => c.Account)
            .Where(c => c.ParentCategoryId == category.Id && c.IsActive && c.Type == category.Type)
            .ToListAsync();
        foreach (var subCategory in category.Categories)
        {
            subCategory.Categories = await GetSubCategories(subCategory);
        }

        return category.Categories.ToList();
    }
}