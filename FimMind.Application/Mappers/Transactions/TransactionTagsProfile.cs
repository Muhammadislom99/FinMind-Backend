using FinMind.Application.Contract.Transactions.Tags.Responses;

namespace FimMind.Application.Mappers.Transactions;

public class TransactionTagsProfile : Profile
{
    public TransactionTagsProfile()
    {
        CreateMap<TransactionTag, TransactionTagsResponse>()
            .ForMember(dest => dest.TransactionId,
                opt => opt
                    .MapFrom(src => src.TransactionId))
            .ForMember(dest => dest.Tags,
                opt => opt
                    .MapFrom(src => src.Tag.Name));

    }
}