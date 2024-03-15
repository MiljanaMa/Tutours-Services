using Explorer.API.Controllers.Tourist.Commenting;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public.Commenting;
using Explorer.BuildingBlocks.Core.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration.Commenting;

[Collection("Sequential")]
public class CommentQueryTests : BaseBlogIntegrationTest
{
    public CommentQueryTests(BlogTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Retrieve_all()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = ((ObjectResult)controller.GetAll(0, 0, -1).Result)?.Value as PagedResult<BlogCommentDto>;

        result.ShouldNotBeNull();
        result.Results.Count.ShouldBe(1);
        result.TotalCount.ShouldBe(1);
    }

    private static BlogCommentController CreateController(IServiceScope scope)
    {
        return new BlogCommentController(scope.ServiceProvider.GetRequiredService<IBlogCommentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}