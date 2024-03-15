using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.Enums;

namespace Explorer.Blog.Core.Mappers;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<BlogStatusDto, BlogStatus>().ReverseMap();
        CreateMap<BlogDto, Domain.Blog>().IncludeAllDerived().ForMember(dest => dest.BlogRatings,
                opt => opt.MapFrom(src => src.BlogRatings.Select(a =>
                    new BlogRating(a.BlogId, a.UserId, a.CreationTime, Enum.Parse<Rating>(a.Rating)))))
            .ReverseMap();
    }
}