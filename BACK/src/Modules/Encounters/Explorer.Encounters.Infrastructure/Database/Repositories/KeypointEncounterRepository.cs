using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.Core.Domain.Enums;
using Explorer.Encounters.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class KeypointEncounterRepository : CrudDatabaseRepository<KeypointEncounter, EncountersContext>, IKeypointEncounterRepository
    {
        private readonly DbSet<KeypointEncounter> _dbSet;

        public KeypointEncounterRepository(EncountersContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<KeypointEncounter>();
        }
        public PagedResult<KeypointEncounter> GetPagedByKeypoint(int page, int pageSize, long keypointId)
        {
            var task = _dbSet.Where(ec => ec.KeyPointId == keypointId)
                             .Include("Encounter")
                             .GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }
        public KeypointEncounter Get(long keypointEncounterId)
        {
            KeypointEncounter keypointEncounter = _dbSet.Where(ec => ec.Id == keypointEncounterId)
                             .Include("Encounter")
                             .FirstOrDefault();
            return keypointEncounter;
        }
        public List<KeypointEncounter> GetAllByKeypoint(long keypointId)
        {
            List<KeypointEncounter> keypointEncounters = _dbSet.Where(ec => ec.KeyPointId == keypointId)
                                                               .Include("Encounter")
                                                               .AsNoTracking()
                                                               .ToList();
            return keypointEncounters;
        }
    }
}
