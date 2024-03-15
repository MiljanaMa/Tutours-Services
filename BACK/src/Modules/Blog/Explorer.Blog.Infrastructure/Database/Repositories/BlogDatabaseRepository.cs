using Explorer.Blog.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Blog.Infrastructure.Database.Repositories;

public class BlogDatabaseRepository : CrudDatabaseRepository<Core.Domain.Blog, BlogContext>, IBlogRepository
{
    private readonly DbSet<Core.Domain.Blog> _dbSet;

    public BlogDatabaseRepository(BlogContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<Core.Domain.Blog>();
    }

    public Core.Domain.Blog GetBlog(long id)
    {
        var entity = _dbSet.AsNoTracking().FirstOrDefault(f => f.Id == id);
        if (entity == null) throw new KeyNotFoundException("Not found: " + id);
        return entity;
    }

    public PagedResult<Core.Domain.Blog> GetWithStatuses(int page, int pageSize)
    {
        var task = _dbSet.Include(b => b.BlogStatuses).GetPaged(page, pageSize);
        task.Wait();
        return task.Result;
    }
}