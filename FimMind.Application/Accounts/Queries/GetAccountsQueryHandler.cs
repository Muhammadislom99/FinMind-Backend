using FinMind.Application.Contract.Accounts.Queries;
using FinMind.Application.Contract.Accounts.Responses;
using FinMind.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Accounts.Queries;

public class GetAccountsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetAccountsQuery, List<AccountResponse>>
{
    public async Task<List<AccountResponse>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await dbContext.Accounts.Where(x => x.UserId == CurrentUser.Id && x.IsActive &&
                                                           (x.Type == AccountType.Cash ||
                                                            x.Type == AccountType.CreditCard ||
                                                            x.Type == AccountType.MobileWallet)
        ).ToListAsync(cancellationToken);
       
        return mapper.Map<List<AccountResponse>>(accounts);
        
    }
}