
using FinMind.Application.Contract.Transactions.Expenses.Commands;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Transaction = FinMind.Domain.Entities.Transaction;

namespace FimMind.Application.Transactions.Commands;

public class CreateExpenseTransactionCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateExpenseTransactionCommand, string>
{
    public async Task<string> Handle(CreateExpenseTransactionCommand request, CancellationToken cancellationToken)
    {
        await using var transact = await dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == request.AccountId
                                                                            && a.UserId == CurrentUser.Id
                                                                            && (a.Type == AccountType.Cash ||
                                                                                a.Type == AccountType.CreditCard ||
                                                                                a.Type == AccountType.MobileWallet),
                cancellationToken);
            if (account == null)
                throw new NotFoundException("Account not found");

            var category = await dbContext.Categories.Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId 
                                           && c.Account.UserId==CurrentUser.Id , cancellationToken);

            if (category == null)
                throw new NotFoundException("Category not found");

            if (account.Balance < request.Amount)
                throw new TransactionsException("Insufficient balance");

            account.Balance -= request.Amount;
            category.Account.Balance += request.Amount;

            dbContext.Transactions.Add(new Transaction()
            {
                Amount = request.Amount,
                FromAccountId = account.Id,
                ToAccountId = category.AccountId,
                Type = TransactionType.Expense
            });

            await dbContext.SaveChangesAsync(cancellationToken);
            await transact.CommitAsync(cancellationToken);
            return $"Expense of {request.Amount:N2}{account.Currency} recorded successfully.";
        }
        catch (Exception e)
        {
            await transact.RollbackAsync(cancellationToken);
            throw;
        }
    }
}