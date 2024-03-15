using AutoMapper;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.Core.Domain;

namespace Explorer.Blog.Core.Mappers;

public class BlogCommentProfile : Profile
{
    public BlogCommentProfile()
    {
        CreateMap<BlogCommentDto, BlogComment>().ReverseMap();
    }
}