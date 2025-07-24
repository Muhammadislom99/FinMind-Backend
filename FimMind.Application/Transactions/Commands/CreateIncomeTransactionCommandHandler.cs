using FinMind.Application.Contract.Transactions.Incomes.Commands;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Transaction = FinMind.Domain.Entities.Transaction;

namespace FimMind.Application.Transactions.Commands;

public class CreateIncomeTransactionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateIncomeTransactionCommand, string>
{
    public async Task<string> Handle(CreateIncomeTransactionCommand request, CancellationToken cancellationToken)
    {
        await using var transact = await dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var category =
                await dbContext.Categories
                    .Include(c => c.Account)
                    .FirstOrDefaultAsync(
                        c => c.Id == request.CategoryId
                             && c.Type == CategoryType.Income
                             && c.Account.UserId == CurrentUser.Id, cancellationToken);

            if (category == null)
                throw new NotFoundException("Category not found");

            var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == request.AccountId
                                                                            && a.UserId == CurrentUser.Id
                                                                            && (a.Type == AccountType.Cash ||
                                                                                a.Type == AccountType.CreditCard ||
                                                                                a.Type == AccountType.MobileWallet),
                cancellationToken);
            if (account == null)
                throw new NotFoundException("Account not found");

            account.Balance += request.Amount;
            category.Account.Balance += request.Amount;

            var transaction = new Transaction()
            {
                FromAccountId = category.AccountId,
                ToAccountId = account.Id,
                Amount = request.Amount,
                Type = TransactionType.Income,
                DateTime = request.DateTime ?? DateTime.Now,
                Description = request.Description
            };

            dbContext.Transactions.Add(transaction);

            await dbContext.SaveChangesAsync(cancellationToken);

            await transact.CommitAsync(cancellationToken);
            return $"Income of {request.Amount:N2}{account.Currency} recorded successfully.";
            ;
        }
        catch (Exception e)
        {
            await transact.RollbackAsync(cancellationToken);
            throw;
        }
    }
}