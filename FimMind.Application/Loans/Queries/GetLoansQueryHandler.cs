using FinMind.Application.Contract.Loans.Queries;
using FinMind.Application.Contract.Loans.Responses;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Loans.Queries;

public class GetLoansQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetLoansQuery, List<LoanResponse>>
{
    public async Task<List<LoanResponse>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
    {
        var loans = await dbContext.Loans
            .Include(x => x.Account)
            .Where(x => x.Account.UserId == CurrentUser.Id && x.Status == LoanStatus.Active)
            .ToListAsync(cancellationToken);

        // var ids = loans.Select(x => x.AccountId).ToArray();
        //
        // var totalPaidByAccount = await dbContext.Transactions
        //     .Where(t => ids.Contains(t.ToAccountId))
        //     .GroupBy(t => t.ToAccountId)
        //     .Select(g => new { AccountId = g.Key, TotalPaid = g.Sum(t => t.Amount) })
        //     .ToDictionaryAsync(k => k.AccountId, v => v.TotalPaid, cancellationToken);

        return loans.Select(mapper.Map<LoanResponse>).ToList();
    }
}