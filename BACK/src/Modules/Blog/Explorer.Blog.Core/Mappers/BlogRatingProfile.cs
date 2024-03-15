using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;

namespace Explorer.Blog.Core.Mappers;

public class BlogRatingProfile : Profile
{
    public BlogRatingProfile()
    {
        CreateMap<BlogRatingDto, BlogRating>().ReverseMap();
    }
}