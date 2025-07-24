using FinMind.Application.Contract.Transactions.Transfer.Commands;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Transaction = FinMind.Domain.Entities.Transaction;

namespace FimMind.Application.Transactions.Commands;

public class CreateTransferTransactionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateTransferTransactionCommand, string>
{
    public async Task<string> Handle(CreateTransferTransactionCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            if (request.FromAccountId == request.ToAccountId)
                throw new TransactionsException("Cannot transfer to the same account");

            var fromAccount = await dbContext.Accounts.SingleOrDefaultAsync(
                a => a.Id == request.FromAccountId && a.UserId == CurrentUser.Id,
                cancellationToken);

            var toAccount = await dbContext.Accounts.SingleOrDefaultAsync(
                a => a.Id == request.ToAccountId && a.UserId == CurrentUser.Id,
                cancellationToken);

            if (fromAccount == null)
                throw new NotFoundException("Source account not found");

            if (toAccount == null)
                throw new NotFoundException("Destination account not found");

            if (!IsAccountTypeSupported(fromAccount.Type))
                throw new TransactionsException("Source account type not supported");

            if (!IsAccountTypeSupported(toAccount.Type))
                throw new TransactionsException("Destination account type not supported");

            if (fromAccount.Balance < request.Amount)
                throw new TransactionsException("Insufficient balance");

            fromAccount.Balance -= request.Amount;
            toAccount.Balance += request.Amount;

            dbContext.Transactions.Add(new Transaction
            {
                FromAccountId = fromAccount.Id,
                ToAccountId = toAccount.Id,
                Amount = request.Amount,
                Type = TransactionType.Transfer,
                DateTime = request.DateTime ?? DateTime.UtcNow,
                Description = request.Description
            });

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return $"Transfer of {request.Amount:N2}{fromAccount.Currency} completed successfully.";
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static bool IsAccountTypeSupported(AccountType type) =>
        type is AccountType.Cash or AccountType.CreditCard or AccountType.MobileWallet;
}