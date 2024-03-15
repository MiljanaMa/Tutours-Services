using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class PublicKeypointRepository : CrudDatabaseRepository<PublicKeypoint, ToursContext>, IPublicKeypointRepository
{
    private readonly DbSet<PublicKeypoint> _dbSet;

    public PublicKeypointRepository(ToursContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<PublicKeypoint>();
    }

    public PagedResult<PublicKeypoint> GetPagedInRange(int page, int pageSize, double longitude, double latitude,
        double radius)
    {
        double earthRadius = 6371;
        var degreeRad = Math.PI / 180.0;

        var longDegree = longitude * degreeRad;
        var latDegree = latitude * degreeRad;


        // HEAR MY WORDS THAT I MIGHT TEACH UUU
        // TAKEE MY ARMSS THAT I MIGHT REACH UUUUU
        // BUUUUUT MY WORDDDDDS  LIKE SILENT  RAINDROPSSSS FEEELLLLLL
        var task = _dbSet
            .Where(kp =>
                2 * earthRadius *
                Math.Atan2(
                    Math.Sqrt(
                        Math.Sin((kp.Latitude * degreeRad - latDegree) / 2) *
                        Math.Sin((kp.Latitude * degreeRad - latDegree) / 2)
                        +
                        Math.Cos(kp.Latitude * degreeRad) * Math.Cos(latDegree)
                                                          *
                                                          Math.Sin((kp.Longitude * degreeRad - longDegree) / 2) *
                                                          Math.Sin((kp.Longitude * degreeRad - longDegree) / 2)
                    )
                    ,
                    Math.Sqrt(
                        1
                        -
                        (
                            Math.Sin((kp.Latitude * degreeRad - latDegree) / 2) *
                            Math.Sin((kp.Latitude * degreeRad - latDegree) / 2)
                            +
                            Math.Cos(kp.Latitude * degreeRad) * Math.Cos(latDegree)
                                                              *
                                                              Math.Sin((kp.Longitude * degreeRad - longDegree) / 2) *
                                                              Math.Sin((kp.Longitude * degreeRad - longDegree) / 2)
                        )
                    )
                )
                <= radius)
            .GetPagedById(page, pageSize);
        task.Wait();
        return task.Result;
    }
}