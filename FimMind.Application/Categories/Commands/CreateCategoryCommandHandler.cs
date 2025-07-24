using FinMind.Application.Contract.Categories.Commands;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Categories.Commands;

public class CreateCategoryCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateCategoryCommand, bool>
{
    public async Task<bool> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var tansact = await dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var categoty = await dbContext.Categories
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.Type == (CategoryType)request.Type
                                          && c.Account.Name.ToLower() == request.Name.Trim().ToLower()
                    , cancellationToken);
            if (categoty is not null) throw new ExistException("Category with the given name already exists.");

            if (request.ParentCategoryId.HasValue)
            {
                var parent = await dbContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.ParentCategoryId, cancellationToken);
                if (parent == null) throw new NotFoundException("Parent category not found.");
            }

            var account = new Account()
            {
                Name = request.Name.Trim(),
                Description = request.Description,
                Type = (CategoryType)request.Type == CategoryType.Expense
                    ? AccountType.Expense
                    : AccountType.Income,
                UserId = CurrentUser.Id
            };
            dbContext.Accounts.Add(account);
            dbContext.Categories.Add(new Category()
            {
                AccountId = account.Id,
                Icon = request.Icon,
                Color = request.Color,
                ParentCategoryId = request.ParentCategoryId,
                Type = (CategoryType)request.Type,
            });
            await dbContext.SaveChangesAsync(cancellationToken);
            await tansact.CommitAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            await tansact.RollbackAsync(cancellationToken);
            Console.WriteLine(e);
            throw;
        }
    }
}