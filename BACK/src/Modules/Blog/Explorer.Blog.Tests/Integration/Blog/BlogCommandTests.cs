using Explorer.API.Controllers.Tourist.Blog;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Dtos.Enums;
using Explorer.Blog.API.Public.Blog;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration.Blog;

[Collection("Sequential")]
public class BlogCommandTests : BaseBlogIntegrationTest
{
    public BlogCommandTests(BlogTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var newEntity = new BlogDto
        {
            CreatorId = -11,
            Title = "Breathtaking visit to Dubai",
            Description = "Simply breathtaking.",
            CreationDate = DateOnly.FromDateTime(DateTime.Now.ToUniversalTime()),
            ImageLinks = new List<string> { "test" },
            SystemStatus = BlogSystemStatus.DRAFT.ToString(),
            BlogRatings = new List<BlogRatingDto>()
        };

        //Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as BlogDto;

        //Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Id.ShouldBe(result.Id);

        //Assert - Database
        var storedEntity = dbContext.Blogs.FirstOrDefault(i => i.Title == newEntity.Title);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new BlogDto
        {
            Title = "",
            Description = "",
            SystemStatus = ""
        };

        //Act
        var result = (ObjectResult)controller.Create(updatedEntity).Result;

        //Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(400);
    }

    [Fact]
    public void Updates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var updatedEntity = new BlogDto
        {
            Id = -1,
            CreatorId = -1,
            Title = "Spectacular visit to Russia",
            Description = "Spectacular!",
            CreationDate = DateOnly.FromDateTime(DateTime.Now.ToUniversalTime()),
            ImageLinks = new List<string> { "img1.jpg" },
            SystemStatus = BlogSystemStatus.DRAFT.ToString()
        };

        //Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as BlogDto;

        //Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-1);
        result.Title.ShouldBe(updatedEntity.Title);
        result.Description.ShouldBe(updatedEntity.Description);
        result.CreationDate.ShouldBe(updatedEntity.CreationDate);
        result.ImageLinks.ShouldBe(updatedEntity.ImageLinks);
        result.SystemStatus.ShouldBe(updatedEntity.SystemStatus);

        //Assert - Database
        var storedEntity = dbContext.Blogs.FirstOrDefault(i => i.Description == "Spectacular!");
        storedEntity.ShouldNotBeNull();
        storedEntity.Title.ShouldBe(updatedEntity.Title);
        var oldEntity = dbContext.Blogs.FirstOrDefault(i => i.Description == "It was wonderful");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new BlogDto
        {
            Id = -420,
            Title = "Invalid update",
            Description = "Cannot happen",
            SystemStatus = ""
        };

        //Act
        var result = (ObjectResult)controller.Update(updatedEntity).Result;

        //Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    [Fact]
    public void Deletes()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();

        //Act
        var result = (OkResult)controller.Delete(-3);

        //Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        //Assert - Database
        var deletedEntity = dbContext.Blogs.FirstOrDefault(i => i.Id == -3);
        deletedEntity.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        //Act
        var result = (ObjectResult)controller.Delete(-420);

        //Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static BlogController CreateController(IServiceScope scope)
    {
        return new BlogController(scope.ServiceProvider.GetRequiredService<IBlogService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}