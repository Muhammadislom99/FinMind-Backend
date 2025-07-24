using FinMind.Application.Contract.Categories.Commands;
using FinMind.Application.Contract.Categories.Responses;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Categories.Commands;

public class UpdateCategoryCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.Account.UserId == CurrentUser.Id, cancellationToken);
        if (category is null)
            throw new NotFoundException("Category not found");

        var existName = await dbContext.Categories
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => (c.Id != request.Id 
                                      && c.Account.Name.ToLower() == request.Name.Trim().ToLower()
                                      && c.Type == category.Type && c.Account.UserId == CurrentUser.Id),
                cancellationToken);
        if (existName != null)
            throw new NotFoundException("Category with same name already exists");

        category.Account.Name = request.Name;
        category.Account.Description = !string.IsNullOrEmpty(request.Description)
            ? request.Description
            : category.Account.Description;
        category.Icon = !string.IsNullOrEmpty(request.Icon)
            ? request.Icon
            : category.Icon;
        category.Color = !string.IsNullOrEmpty(request.Color)
            ? request.Color
            : category.Color;

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<CategoryResponse>(category);
    }
}