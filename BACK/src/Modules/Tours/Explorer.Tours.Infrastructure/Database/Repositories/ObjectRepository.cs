using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain.Enum;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Object = Explorer.Tours.Core.Domain.Object;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class ObjectRepository : CrudDatabaseRepository<Object, ToursContext>, IObjectRepository
{
    private readonly DbSet<Object> _dbSet;

    public ObjectRepository(ToursContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<Object>();
    }

    public PagedResult<Object> GetPublicPaged(int page, int pageSize)
    {
        var task = _dbSet.Where(o => o.Status == ObjectStatus.PUBLIC).GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }

    public PagedResult<Object> GetPublicPagedInRange(int page, int pageSize, double longitude, double latitude,
        double radius)
    {
        double earthRadius = 6371;
        var degreeRad = Math.PI / 180.0;

        var longDegree = longitude * degreeRad;
        var latDegree = latitude * degreeRad;


        // hello darkness my old friend
        // I've come to talk with you againnnnn
        var task = _dbSet
            .Where(o =>
                o.Status == ObjectStatus.PUBLIC
                &&
                2 * earthRadius *
                Math.Atan2(
                    Math.Sqrt(
                        Math.Sin((o.Latitude * degreeRad - latDegree) / 2) *
                        Math.Sin((o.Latitude * degreeRad - latDegree) / 2)
                        +
                        Math.Cos(o.Latitude * degreeRad) * Math.Cos(latDegree)
                                                         *
                                                         Math.Sin((o.Longitude * degreeRad - longDegree) / 2) *
                                                         Math.Sin((o.Longitude * degreeRad - longDegree) / 2)
                    )
                    ,
                    Math.Sqrt(
                        1
                        -
                        (
                            Math.Sin((o.Latitude * degreeRad - latDegree) / 2) *
                            Math.Sin((o.Latitude * degreeRad - latDegree) / 2)
                            +
                            Math.Cos(o.Latitude * degreeRad) * Math.Cos(latDegree)
                                                             *
                                                             Math.Sin((o.Longitude * degreeRad - longDegree) / 2) *
                                                             Math.Sin((o.Longitude * degreeRad - longDegree) / 2)
                        )
                    )
                )
                <= radius)
            .GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }
}