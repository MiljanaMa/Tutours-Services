using Explorer.API.Controllers.Tourist.Blog;
using Explorer.Blog.API.Dtos;
using Explorer.Blog.API.Public.Blog;
using Explorer.Blog.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Explorer.Blog.Tests.Integration.Blog;

[Collection("Sequential")]
public class BlogStatusCommandTests : BaseBlogIntegrationTest
{
    public BlogStatusCommandTests(BlogTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Creates()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogContext>();
        var newEntity = new BlogStatusDto
        {
            BlogId = -1,
            Name = "famous"
        };

        //Act
        var result = ((ObjectResult)controller.Create(newEntity).Result)?.Value as BlogStatusDto;

        //Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldNotBe(0);
        result.Id.ShouldBe(result.Id);

        //Assert - Database
        var storedEntity = dbContext.BlogStatuses.FirstOrDefault(i => i.Name == newEntity.Name);
        storedEntity.ShouldNotBeNull();
        storedEntity.Id.ShouldBe(result.Id);
    }

    [Fact]
    public void Create_fails_invalid_data()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new BlogStatusDto
        {
            Name = ""
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
        var updatedEntity = new BlogStatusDto
        {
            Id = -2,
            BlogId = -1,
            Name = "active"
        };

        //Act
        var result = ((ObjectResult)controller.Update(updatedEntity).Result)?.Value as BlogStatusDto;

        //Assert - Response
        result.ShouldNotBeNull();
        result.Id.ShouldBe(-2);
        result.Name.ShouldBe("active");

        //Assert - Database
        var storedEntity = dbContext.BlogStatuses.FirstOrDefault(i => i.Name == "active");
        storedEntity.ShouldNotBeNull();
        var oldEntity = dbContext.BlogStatuses.FirstOrDefault(i => i.Name == "famous");
        oldEntity.ShouldBeNull();
    }

    [Fact]
    public void Update_fails_invalid_id()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);
        var updatedEntity = new BlogStatusDto
        {
            Id = -87,
            BlogId = -1,
            Name = "active"
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
        var result = (OkResult)controller.Delete(-1);

        //Assert - Response
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(200);

        //Assert - Database
        var deletedEntity = dbContext.BlogStatuses.FirstOrDefault(i => i.Id == -1);
        deletedEntity.ShouldBeNull();
    }

    [Fact]
    public void Delete_fails_invalid_id()
    {
        //Arrange
        using var scope = Factory.Services.CreateScope();
        var controller = CreateController(scope);

        //Act
        var result = (ObjectResult)controller.Delete(-97);

        //Assert
        result.ShouldNotBeNull();
        result.StatusCode.ShouldBe(404);
    }

    private static BlogStatusController CreateController(IServiceScope scope)
    {
        return new BlogStatusController(scope.ServiceProvider.GetRequiredService<IBlogStatusService>())
        {
            ControllerContext = BuildContext("-1")
        };
    }
}