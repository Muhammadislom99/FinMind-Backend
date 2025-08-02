using FinMind.Application.Contract.Loans.Commands;
using FinMind.Application.Contract.Loans.Responses;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Loans.Commands;

public class CreateLoanCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<CreateLoanCommand, LoanResponse>
{
    public async Task<LoanResponse> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
    {
        await using var transact = await dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var loan = await dbContext.Loans
                .Include(l => l.Account)
                .FirstOrDefaultAsync(l => l.Account.Name == request.Name
                                          && l.Account.UserId == CurrentUser.Id
                                          && l.Account.Type == AccountType.Loan,
                    cancellationToken);

            if (loan != null)
                throw new ExistException("Loan already exists");

            var account = new Account
            {
                Name = request.Name,
                Description = request.Description,
                Type = AccountType.Loan,
                UserId = CurrentUser.Id,
            };
            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync(cancellationToken);
            
            loan = new Loan
            {
                Amount = request.Amount,
                AccountId = account.Id,
                PrincipalAmount = request.PrincipalAmount,
                InterestRate = request.InterestRate,
                EndDate = request.EndDate,
                StartDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
                MonthlyPayment = request.MonthlyPayment
            };
            dbContext.Loans.Add(loan);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transact.CommitAsync(cancellationToken);

            return mapper.Map<LoanResponse>(loan);
        }
        catch (Exception)
        {
            await transact.RollbackAsync(cancellationToken);
            throw;
        }
    }
}