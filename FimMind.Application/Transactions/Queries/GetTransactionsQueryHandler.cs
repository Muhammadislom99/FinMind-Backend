using FinMind.Application.Contract.Enums;
using FinMind.Application.Contract.Transactions.Queries;
using FinMind.Application.Contract.Transactions.Responses;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Transactions.Queries;

public class GetTransactionsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetTransactionsQuery, List<TransactionResponse>>
{
    public async Task<List<TransactionResponse>> Handle(GetTransactionsQuery request,
        CancellationToken cancellationToken)
    {
            var allUserAccounts = await dbContext.Accounts
                .IgnoreQueryFilters()
                .Where(a => a.UserId == CurrentUser.Id )
                .Select(ac => new
                {
                    AccountId = ac.Id,
                    AccountName = ac.Name,
                })
                .ToListAsync(cancellationToken);

        var allAccountIds = allUserAccounts.Select(a => a.AccountId).ToList();

        if (request.AccountId != null && !allAccountIds.Contains(request.AccountId.Value))
        {
            throw new NotFoundException("Account", request.AccountId.Value);
        }

        IQueryable<Transaction> transactionsQuery;

        if (request.AccountId != null)
        {
            transactionsQuery = dbContext.Transactions
                .Where(t => t.FromAccountId == request.AccountId || t.ToAccountId == request.AccountId);
        }
        else
        {
            transactionsQuery = dbContext.Transactions
                .Where(t => allAccountIds.Contains(t.FromAccountId) || allAccountIds.Contains(t.ToAccountId));
        }

        if (request.Type != null)
        {
            transactionsQuery = transactionsQuery
                .Where(t => t.Type == (FinMind.Domain.Enums.TransactionType)request.Type);
        }

        if (request is { StartDate: not null, EndDate: not null })
        {
            var startDate = request.StartDate.Value.ToDateTime(TimeOnly.MinValue);
            var endDate = request.EndDate.Value.AddDays(1).ToDateTime(TimeOnly.MinValue);

            transactionsQuery = transactionsQuery.Where(t =>
                t.DateTime >= startDate &&
                t.DateTime < endDate);
        }
        else if (request.StartDate != null)
        {
            var startDate = request.StartDate.Value.ToDateTime(TimeOnly.MinValue);
            var endDate = startDate.AddDays(1);
            transactionsQuery = transactionsQuery.Where(t =>
                t.DateTime >= startDate &&
                t.DateTime < endDate);
        }
        else if (request.EndDate != null)
        {
            var endDate = request.EndDate.Value.ToDateTime(TimeOnly.MinValue);
            var nextDay = endDate.AddDays(1);
            transactionsQuery = transactionsQuery.Where(t =>
                t.DateTime >= endDate &&
                t.DateTime < nextDay);
        }

        var transactions = await transactionsQuery
            .Select(tr => new
            {
                Id = tr.Id,
                FromAccountId = tr.FromAccountId,
                ToAccountId = tr.ToAccountId,
                DateTime = tr.DateTime,
                Description = tr.Description,
                Amount = tr.Amount,
                Type = tr.Type,
                Tags = tr.TransactionTags.Select(t => t.Tag.Name).ToArray()
            })
            .OrderByDescending(t => t.DateTime)
            .ToListAsync(cancellationToken);

        var result = new List<TransactionResponse>();

        foreach (var tr in transactions)
        {
            var response = new TransactionResponse()
            {
                Id = tr.Id,
                Description = tr.Description,
                Amount = tr.Amount,
                Type = (TransactionType)tr.Type,
                DateTime = tr.DateTime,
                Tags = tr.Tags.ToList()
            };

            var fromAccount = allUserAccounts.FirstOrDefault(a => a.AccountId == tr.FromAccountId);
            var toAccount = allUserAccounts.FirstOrDefault(a => a.AccountId == tr.ToAccountId);

            switch (response.Type)
            {
                case TransactionType.Expense:
                    response.Title = toAccount != null
                        ? $"Расход на {toAccount.AccountName}"
                        : "Расход (внешний счет)";
                    response.SubTitle = fromAccount != null
                        ? $"C счёта {fromAccount.AccountName}"
                        : "С внешнего счета";
                    break;

                case TransactionType.Income:
                    response.Title = fromAccount != null
                        ? $"Доход от {fromAccount.AccountName}"
                        : "Доход (внешний источник)";
                    response.SubTitle = toAccount != null
                        ? $"На счет {toAccount.AccountName}"
                        : "На внешний счет";
                    break;

                case TransactionType.Transfer:
                    var fromName = fromAccount?.AccountName ?? "Внешний счет";
                    var toName = toAccount?.AccountName ?? "Внешний счет";
                    response.Title = $"Между счетами {fromName} -> {toName}";
                    break;

                case TransactionType.Repayment:
                    response.Title = toAccount != null
                        ? $"Погашение {toAccount.AccountName}"
                        : "Погашение (внешний счет)";
                    response.SubTitle = fromAccount != null
                        ? $"C счета {fromAccount.AccountName}"
                        : "С внешнего счета";
                    break;

                case TransactionType.Saving:
                    response.Title = toAccount != null
                        ? $"Накопление на {toAccount.AccountName}"
                        : "Накопление (внешний счет)";
                    response.SubTitle = fromAccount != null
                        ? $"С счета {fromAccount.AccountName}"
                        : "С внешнего счета";
                    break;
            }

            result.Add(response);
        }

        return result;
    }
}