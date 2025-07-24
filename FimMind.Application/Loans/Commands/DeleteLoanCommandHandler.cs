using FinMind.Application.Contract.Loans.Commands;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Loans.Commands;

public class DeleteLoanCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteLoanCommand, bool>
{
    public async Task<bool> Handle(DeleteLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = dbContext.Loans
            .Include(x => x.Account)
            .FirstOrDefault(x => x.Id == request.Id && x.Account.UserId == CurrentUser.Id);

        if (loan == null)
            throw new NotFoundException("loan not found");

        dbContext.Accounts.Remove(loan.Account);
        dbContext.Loans.Remove(loan);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
        ;
    }
}