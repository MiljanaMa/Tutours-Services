using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.Core.Domain.Enums;


namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class EncounterRepository : CrudDatabaseRepository<Encounter, EncountersContext>, IEncounterRepository
    {
        private readonly DbSet <Encounter> _dbSet;

        public EncounterRepository(EncountersContext dbContext) : base (dbContext)
        {
            _dbSet = dbContext.Set<Encounter>();
        }

        public PagedResult<Encounter> GetApproved(int page, int pageSize)
        {
            var task = _dbSet.AsNoTracking().Where(e => e.ApprovalStatus == EncounterApprovalStatus.SYSTEM_APPROVED || e.ApprovalStatus == EncounterApprovalStatus.ADMIN_APPROVED).GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public PagedResult<Encounter> GetApprovedByStatus(int page, int pageSize, EncounterStatus status)
        {
            var task = _dbSet.AsNoTracking().Where(e => e.Status == status
                            && (e.ApprovalStatus == EncounterApprovalStatus.SYSTEM_APPROVED || e.ApprovalStatus == EncounterApprovalStatus.ADMIN_APPROVED)).GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public IEnumerable<Encounter> GetApprovedByStatusAndType(EncounterStatus status, EncounterType type)
        {
            return _dbSet.AsNoTracking().Where(e => e.Status == status && e.Type == type
                            && (e.ApprovalStatus == EncounterApprovalStatus.SYSTEM_APPROVED || e.ApprovalStatus == EncounterApprovalStatus.ADMIN_APPROVED));
        }

        public PagedResult<Encounter> GetByUser(int page, int pageSize, long userId)
        {
            var task = _dbSet.Where(e => e.UserId == userId).GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public PagedResult<Encounter> GetTouristCreatedEncounters(int page, int pageSize)
        {
            var task = _dbSet.Where(e => e.ApprovalStatus != EncounterApprovalStatus.SYSTEM_APPROVED).GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public PagedResult<Encounter> GetNearbyByType(int page, int pageSize, double longitude, double latitude, EncounterType type)
        {
            double earthRadius = 6371;
            var degreeRad = Math.PI / 180.0;

            var longDegree = longitude * degreeRad;
            var latDegree = latitude * degreeRad;

            var task = _dbSet
                .Where(e =>
                    2 * earthRadius *
                    Math.Atan2(
                        Math.Sqrt(
                            Math.Sin((e.Latitude * degreeRad - latDegree) / 2) *
                            Math.Sin((e.Latitude * degreeRad - latDegree) / 2)
                            +
                            Math.Cos(e.Latitude * degreeRad) * Math.Cos(latDegree)
                                                              *
                                                              Math.Sin((e.Longitude * degreeRad - longDegree) / 2) *
                                                              Math.Sin((e.Longitude * degreeRad - longDegree) / 2)
                        )
                        ,
                        Math.Sqrt(
                            1
                            -
                            (
                                Math.Sin((e.Latitude * degreeRad - latDegree) / 2) *
                                Math.Sin((e.Latitude * degreeRad - latDegree) / 2)
                                +
                                Math.Cos(e.Latitude * degreeRad) * Math.Cos(latDegree)
                                                                  *
                                                                  Math.Sin((e.Longitude * degreeRad - longDegree) / 2) *
                                                                  Math.Sin((e.Longitude * degreeRad - longDegree) / 2)
                            )
                        )
                    )
                    <= e.Range
                    && e.Type == type)
                .GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public PagedResult<Encounter> GetNearby(int page, int pageSize, double longitude, double latitude)
        {
            double earthRadius = 6371;
            var degreeRad = Math.PI / 180.0;

            var longDegree = longitude * degreeRad;
            var latDegree = latitude * degreeRad;

            var task = _dbSet
                .Where(e =>
                    2 * earthRadius *
                    Math.Atan2(
                        Math.Sqrt(
                            Math.Sin((e.Latitude * degreeRad - latDegree) / 2) *
                            Math.Sin((e.Latitude * degreeRad - latDegree) / 2)
                            +
                            Math.Cos(e.Latitude * degreeRad) * Math.Cos(latDegree)
                                                              *
                                                              Math.Sin((e.Longitude * degreeRad - longDegree) / 2) *
                                                              Math.Sin((e.Longitude * degreeRad - longDegree) / 2)
                        )
                        ,
                        Math.Sqrt(
                            1
                            -
                            (
                                Math.Sin((e.Latitude * degreeRad - latDegree) / 2) *
                                Math.Sin((e.Latitude * degreeRad - latDegree) / 2)
                                +
                                Math.Cos(e.Latitude * degreeRad) * Math.Cos(latDegree)
                                                                  *
                                                                  Math.Sin((e.Longitude * degreeRad - longDegree) / 2) *
                                                                  Math.Sin((e.Longitude * degreeRad - longDegree) / 2)
                            )
                        )
                    )
                    <= e.Range)
                .GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }
    }
}
