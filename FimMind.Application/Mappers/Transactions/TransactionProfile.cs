using FinMind.Application.Contract.Transactions.Responses;
using Transaction = FinMind.Domain.Entities.Transaction;

namespace FimMind.Application.Mappers.Transactions;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<TransactionResponse, Transaction>();
    }
}