using FinMind.Application.Contract.Loans.Responses;

namespace FimMind.Application.Mappers.Loans;

public class LoanProfile : Profile
{
    public LoanProfile()
    {
        CreateMap<Loan, LoanResponse>()
            .ForMember(dest => dest.Name,
                opt => opt
                    .MapFrom(src => src.Account.Name))
            .ForMember(dest => dest.Description,
                opt => opt
                    .MapFrom(src => src.Account.Description))
            .ForMember(dest => dest.TotalPaid, opt =>
                opt.MapFrom(src => src.Account.Balance));
    }
}