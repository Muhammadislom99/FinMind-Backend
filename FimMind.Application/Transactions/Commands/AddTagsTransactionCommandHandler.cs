using FinMind.Application.Contract.Transactions.Tags.Commands;
using FinMind.Application.Contract.Transactions.Tags.Responses;
using Microsoft.EntityFrameworkCore;

namespace FimMind.Application.Transactions.Commands;

public class AddTagsTransactionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<AddTagsTransactionCommand, TransactionTagsResponse>
{
    public async Task<TransactionTagsResponse> Handle(AddTagsTransactionCommand request,
        CancellationToken cancellationToken)
    {
        await using var transact = await dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var transaction =
                await dbContext.Transactions
                    .Include(t => t.TransactionTags)
                    .ThenInclude(tg => tg.Tag)
                    .FirstOrDefaultAsync(t => t.Id == request.TransactionId,
                        cancellationToken);
            if (transaction == null) throw new NotFoundException("Transaction not found");

            var tagNames = request.Tags.Distinct(StringComparer.CurrentCultureIgnoreCase);

            var existingTags = await dbContext.Tags
                .Where(t => tagNames.Contains(t.Name))
                .ToListAsync(cancellationToken);

            var newTags = tagNames
                .Except(existingTags
                    .Select(t => t.Name), StringComparer.CurrentCultureIgnoreCase)
                .Select(name => new Tag() { Name = name })
                .ToList();

            await dbContext.Tags.AddRangeAsync(newTags, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var existingTagIds = transaction.TransactionTags
                .Select(tt => tt.TagId)
                .ToHashSet();

            var tagsToLinkFromExisting = existingTags
                .Where(tag => !existingTagIds.Contains(tag.Id))
                .ToList();

            var tagsToLink = tagsToLinkFromExisting.Concat(newTags).ToList();

            var transactionTags = tagsToLink.Select(tag => new TransactionTag
            {
                TransactionId = transaction.Id,
                TagId = tag.Id
            }).ToList();

            await dbContext.TransactionTags.AddRangeAsync(transactionTags, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);

            var result = new TransactionTagsResponse()
            {
                TransactionId = transaction.Id,
                Tags =
                    transactionTags.Concat(transaction.TransactionTags)
                        .Select(t => t.Tag.Name).Distinct()
                        .OrderBy(t => t).ToList()
            };
            await transact.CommitAsync(cancellationToken);
            return result;
        }
        catch (Exception e)
        {
            await transact.RollbackAsync(cancellationToken);
            Console.WriteLine(e);
            throw;
        }
    }
}