using Explorer.Blog.API.Public.Blog;
using Explorer.Blog.API.Public.Commenting;
using Explorer.Blog.Core.Domain;
using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.Blog.Core.Mappers;
using Explorer.Blog.Core.UseCases.Blog;
using Explorer.Blog.Core.UseCases.Commenting;
using Explorer.Blog.Infrastructure.Database;
using Explorer.Blog.Infrastructure.Database.Repositories;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Explorer.Blog.Infrastructure;

public static class BlogStartup
{
    public static IServiceCollection ConfigureBlogModule(this IServiceCollection services)
    {
        // Registers all profiles since it works on the assembly
        services.AddAutoMapper(typeof(BlogProfile).Assembly);
        SetupCore(services);
        SetupInfrastructure(services);
        return services;
    }

    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IBlogCommentService, BlogCommentService>();
        services.AddScoped<IBlogStatusService, BlogStatusService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped(typeof(IBlogRepository), typeof(BlogDatabaseRepository));
        services.AddScoped(typeof(ICrudRepository<BlogComment>),
            typeof(CrudDatabaseRepository<BlogComment, BlogContext>));
        services.AddScoped(typeof(ICrudRepository<BlogStatus>),
            typeof(CrudDatabaseRepository<BlogStatus, BlogContext>));

        services.AddDbContext<BlogContext>(opt =>
            opt.UseNpgsql(DbConnectionStringBuilder.Build("blog"),
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "blog")));
    }
}