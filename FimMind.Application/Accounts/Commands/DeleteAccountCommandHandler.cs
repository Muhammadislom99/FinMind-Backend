using FinMind.Application.Contract.Accounts.Commands;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Accounts.Commands;

public class DeleteAccountCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteAccountCommand, bool>
{
    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == request.Id && a.UserId == CurrentUser.Id
                 && (a.Type==AccountType.Cash || a.Type==AccountType.CreditCard|| a.Type==AccountType.MobileWallet)
                , cancellationToken);

        if (account == null)
            throw new NotFoundException("Account not found");

        var category = await dbContext.Categories
            .Include(c => c.Account)
            .FirstOrDefaultAsync(c => c.AccountId == request.Id && c.Account.UserId == CurrentUser.Id,
                cancellationToken);

        var loan = await dbContext.Loans
            .Include(l => l.Account)
            .FirstOrDefaultAsync(l => l.AccountId == request.Id && l.Account.UserId == CurrentUser.Id,
                cancellationToken);

        var goal = await dbContext.Goals
            .Include(g => g.Account)
            .FirstOrDefaultAsync(g => g.AccountId == request.Id && g.Account.UserId == CurrentUser.Id,
                cancellationToken);

        if (category != null || loan != null || goal != null)
            throw new InvalidOperationException("Cannot delete account because is it a category or loan or goal");

        dbContext.Accounts.Remove(account);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}