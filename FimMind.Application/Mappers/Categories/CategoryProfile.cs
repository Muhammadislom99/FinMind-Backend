using FinMind.Application.Contract.Categories.Responses;


namespace FimMind.Application.Mappers.Categories;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Account.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Account.Description))
            .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.Categories));
    }
}