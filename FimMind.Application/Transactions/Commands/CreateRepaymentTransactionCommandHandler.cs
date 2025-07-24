using FinMind.Application.Contract.Transactions.Repayments.Commands;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Transaction = FinMind.Domain.Entities.Transaction;

namespace FimMind.Application.Transactions.Commands;

public class CreateRepaymentTransactionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateRepaymentTransactionCommand, string>
{
    public async Task<string> Handle(CreateRepaymentTransactionCommand request, CancellationToken cancellationToken)
    {
        await using var transact = await dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var account =
                await dbContext.Accounts.FirstOrDefaultAsync(
                    x => x.Id == request.AccountId && x.UserId == CurrentUser.Id,
                    cancellationToken);
            if (account == null)
                throw new NotFoundException("Account not found");

            var loan = await dbContext.Loans
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == request.LoanId && x.Account.UserId == CurrentUser.Id,
                    cancellationToken);
            if (loan == null)
                throw new NotFoundException("Loan not found");

            if (account.Balance < request.Amount)
                throw new TransactionsException("Account does not have enough balance");

            var amount = (loan.PrincipalAmount - loan.Account.Balance) < request.Amount
                ? loan.PrincipalAmount - loan.Account.Balance
                : request.Amount;

            var transaction = new Transaction()
            {
                FromAccountId = account.Id,
                ToAccountId = loan.AccountId,
                Amount = amount,
                DateTime = request.DateTime ?? DateTime.UtcNow,
                Description = request.Description,
                Type = TransactionType.Repayment
            };
            dbContext.Transactions.Add(transaction);

            account.Balance -= amount;
            loan.Account.Balance += amount;

            var isLoanFullyPaid = loan.PrincipalAmount == loan.Account.Balance;
            if (isLoanFullyPaid)
                loan.Status = LoanStatus.PaidOff;

            await dbContext.SaveChangesAsync(cancellationToken);
            await transact.CommitAsync(cancellationToken);

            if (isLoanFullyPaid)
            {
                return amount < request.Amount
                    ? $"Loan fully paid off.\nProcessed {amount:N2}{account.Currency} out of {request.Amount:N2}{account.Currency} — overpayment not charged."
                    : $"Loan fully paid off.\nPaid amount: {amount:N2}{account.Currency}.";
            }

            return $"Loan partially paid off in the amount of {amount:N2}{account.Currency}.";
        }
        catch (Exception e)
        {
            await transact.RollbackAsync(cancellationToken);
            throw;
        }
    }
}