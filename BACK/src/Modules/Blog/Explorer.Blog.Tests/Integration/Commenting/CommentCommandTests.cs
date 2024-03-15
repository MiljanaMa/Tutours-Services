using Explorer.API.Controllers.Tourist.Commenting;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public.Commenting;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration.Commenting;

[Collection("Sequential")]
public class CommentCommandTests : BaseBlogIntegrationTest
{
    public CommentCommandTests(BlogTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var newEntity = new BlogCommentDto
        {
            BlogId = 1,
            Username = "steva",
            Comment = "test komentar",
            PostTime = DateTime.Now.ToUniversalTime(),
            LastEditTime = DateTime.Now.ToUniversalTime(),
            IsDeleted = false
        };

        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as BlogCommentDto;

        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Id.ShouldBe(result.Id);

        var storedEntity = dbContext.BlogComments.FirstOrDefault(i => i.Comment == newEntity.Comment);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        // Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new BlogCommentDto
        {
            Comment = string.Empty
        };

        // Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        // Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Update()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var updatedEntity = new BlogCommentDto
        {
            Id = -1,
            BlogId = -1,
            UserId = -1,
            Username = "turista1",
            Comment = "azurirani",
            PostTime = DateTime.Now.ToUniversalTime(),
            LastEditTime = DateTime.Now.ToUniversalTime(),
            IsDeleted = false
        };

        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as BlogCommentDto;

        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.BlogId.ShouldBe(updatedEntity.BlogId);
        result.Comment.ShouldBe(updatedEntity.Comment);
        result.PostTime.ShouldBe(updatedEntity.PostTime);
        result.LastEditTime.ShouldBe(updatedEntity.LastEditTime);
        result.IsDeleted.ShouldBe(updatedEntity.IsDeleted);

        var storedEntity = dbContext.BlogComments.FirstOrDefault(i => i.Comment == updatedEntity.Comment);
        storedEntity.ShouldNotBeNull();
        storedEntity.Comment.ShouldBe(updatedEntity.Comment);
        var oldEntity = dbContext.BlogComments.FirstOrDefault(i => i.Comment == "komentar 1");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new BlogCommentDto
        {
            Id = -69,
            BlogId = -1,
            UserId= -1,
            Username = "turista1",
            Comment = "novi komentar",
            PostTime = DateTime.Now.ToUniversalTime(),
            LastEditTime = DateTime.Now.ToUniversalTime(),
            IsDeleted = false
        };

        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Deletes()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

        var result = (OkResult)controller.Delete(-1);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        var storedCourse = dbContext.BlogComments.FirstOrDefault(i => i.Id == -1);
        storedCourse.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        var result = (ObjectResult)controller.Delete(-1000);

        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static BlogCommentController CreateController(IServiceScope scope)
    {
        return new BlogCommentController(scope.ServiceProvider.GetRequiredService<IBlogCommentService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}