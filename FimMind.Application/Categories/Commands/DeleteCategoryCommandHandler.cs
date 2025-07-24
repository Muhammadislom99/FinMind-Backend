using FinMind.Application.Contract.Categories.Commands;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Categories.Commands;

public class DeleteCategoryCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteCategoryCommand, bool>
{
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.Account.UserId == CurrentUser.Id, cancellationToken);

        if (category == null)
            throw new NotFoundException("Category not found");

        dbContext.Categories.Remove(category);
        dbContext.Accounts.Remove(category.Account);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}