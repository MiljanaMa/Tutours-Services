using Explorer.Blog.API.Dtos;
using FluentResults;

namespace Explorer.Blog.API.Public.Rating;

public interface IBlogRateService
{
    Result<BlogRatingDto> Create(BlogRatingDto blogRatingDto);
}