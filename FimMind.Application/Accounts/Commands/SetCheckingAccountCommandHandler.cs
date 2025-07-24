using FinMind.Application.Contract.Accounts.Commands;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Accounts.Commands;

public class SetCheckingAccountCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<SetCheckingAccountCommand, bool>
{
    public async Task<bool> Handle(SetCheckingAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == request.AccountId && a.UserId == CurrentUser.Id
                                                                && (a.Type == AccountType.Cash ||
                                                                    a.Type == AccountType.CreditCard ||
                                                                    a.Type == AccountType.MobileWallet),
                cancellationToken);

        if (account == null)
            throw new NotFoundException("Account not found");
        
        await dbContext.Accounts
            .Where(a => a.UserId == CurrentUser.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(a => a.CheckingAccount, false), cancellationToken);
        
        account.CheckingAccount = true;
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}