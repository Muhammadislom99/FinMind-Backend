using FinMind.Application.Contract.Accounts;
using FinMind.Application.Contract.Accounts.Commands;
using FinMind.Application.Contract.Accounts.Responses;
using FinMind.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Accounts.Commands;

public class CreateAccountCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<CreateAccountCommand, AccountResponse>
{
    public async Task<AccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var exAccount =
            await dbContext.Accounts.FirstOrDefaultAsync(
                a => a.Name == request.Name.Trim() && a.UserId == CurrentUser.Id, cancellationToken);
        
        if (exAccount != null) throw new ExistException("Account already exists");

        var account = mapper.Map<Account>(request);
        account.UserId = CurrentUser.Id;

        await dbContext.Accounts.AddAsync(account, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<AccountResponse>(account);
    }
}