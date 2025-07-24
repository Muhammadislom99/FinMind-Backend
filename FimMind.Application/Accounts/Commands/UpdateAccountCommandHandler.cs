using FinMind.Application.Contract.Accounts;
using FinMind.Application.Contract.Accounts.Commands;
using FinMind.Application.Contract.Accounts.Responses;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Accounts.Commands;

public class UpdateAccountCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<UpdateAccountCommand, AccountResponse>
{
    public async Task<AccountResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (account == null) throw new NotFoundException("Account is not found");

        account.Name = request.Name;
        account.Description = request.Description;

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<AccountResponse>(account);
    }
}