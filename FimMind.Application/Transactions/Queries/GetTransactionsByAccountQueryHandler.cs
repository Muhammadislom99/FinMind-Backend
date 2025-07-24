using FinMind.Application.Contract.Enums;
using FinMind.Application.Contract.Transactions.Queries;
using FinMind.Application.Contract.Transactions.Responses;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Transactions.Queries;

public class GetTransactionsByAccountQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetTransactionsByAccountQuery, List<TransactionResponse>>
{
    public async Task<List<TransactionResponse>> Handle(GetTransactionsByAccountQuery request,
        CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == request.AccountId
                                      && a.UserId == CurrentUser.Id, cancellationToken);
        if (account == null)
            throw new NotFoundException("Account not found");

        var transactions = await dbContext.Transactions
            .Where(t => t.FromAccountId == account.Id || t.ToAccountId == account.Id)
            .Select(tr => new
            {
                Id = tr.Id,
                FromAccountId = tr.FromAccountId,
                ToAccountId = tr.ToAccountId,
                DateTime = tr.DateTime,
                Description = tr.Description,
                Amount = tr.Amount,
                Type = tr.Type,
                Tags = tr.TransactionTags.Select(t => t.Tag.Name).ToList()
            })
            .OrderByDescending(t => t.DateTime)
            .ToListAsync(cancellationToken);

        var ids = transactions
            .SelectMany(t => new[] { t.FromAccountId, t.ToAccountId })
            .Distinct()
            .ToList();

        var accounts = await dbContext.Accounts
            .Where(a => ids.Contains(a.Id))
            .Select(ac => new
            {
                AccountId = ac.Id,
                AccountName = ac.Name,
            }).ToListAsync(cancellationToken);

        var list = new List<TransactionResponse>();
        foreach (var tr in transactions)
        {
            var response = new TransactionResponse()
            {
                Id = tr.Id,
                Description = tr.Description,
                Amount = tr.Amount,
                Type = (TransactionType)tr.Type,
                DateTime = tr.DateTime,
                Tags = tr.Tags
            };
            switch (response.Type)
            {
                case TransactionType.Expense:
                    response.Title = $"Расход на {accounts.First(a => a.AccountId == tr.ToAccountId).AccountName}";
                    response.SubTitle = $"C счёта {accounts.First(a => a.AccountId == tr.FromAccountId).AccountName}";
                    break;
                case TransactionType.Income:
                    response.Title = $"Доход от {accounts.First(a => a.AccountId == tr.FromAccountId).AccountName}";
                    response.SubTitle = $"На счет {accounts.First(a => a.AccountId == tr.ToAccountId).AccountName}";
                    break;
                case TransactionType.Transfer:
                    response.Title =
                        $"Между счетами {accounts.First(a => a.AccountId == tr.FromAccountId).AccountName}->{accounts.First(a => a.AccountId == tr.ToAccountId).AccountName}";
                    break;
                case TransactionType.Repayment:
                    response.Title = $"Погашение {accounts.First(a => a.AccountId == tr.ToAccountId).AccountName}";
                    response.SubTitle = $"C счета {accounts.First(a => a.AccountId == tr.FromAccountId).AccountName}";
                    break;
                case TransactionType.Saving:
                    response.Title = $"Накопление на {accounts.First(a => a.AccountId == tr.ToAccountId).AccountName}";
                    response.SubTitle = $"С счета  {accounts.First(a => a.AccountId == tr.FromAccountId).AccountName}";
                    break;
            }

            list.Add(response);
        }

        return list;
    }
}