using AutoMapper.Configuration.Conventions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.Infrastructure.Database.Repositories
{
    public class EncounterCompletionRepository : CrudDatabaseRepository<EncounterCompletion, EncountersContext>, IEncounterCompletionRepository
    {
        private readonly DbSet<EncounterCompletion> _dbSet;

        public EncounterCompletionRepository(EncountersContext dbContext) : base(dbContext)
        {
            _dbSet = DbContext.Set<EncounterCompletion>();
        }

        public PagedResult<EncounterCompletion> GetPagedByUser(int page, int pageSize, long userId)
        {
            var task = _dbSet.Where(ec => ec.UserId == userId).Include(ec => ec.Encounter).GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }

        public EncounterCompletion GetByUserAndEncounter(long userId, long encounterId)
        {
            var encounterCompletion = _dbSet.FirstOrDefault(ec => ec.UserId == userId && ec.EncounterId == encounterId);
            return encounterCompletion;
        }
        public EncounterCompletion GetByEncounter(long encounterId)
        {
            var encounterCompletion = _dbSet.FirstOrDefault(ec => ec.EncounterId == encounterId);
            return encounterCompletion;
        }

        public bool HasUserStartedEncounter(long userId, long encounterId) 
        {
            var encounterCompletion = _dbSet.FirstOrDefault(ec => ec.UserId == userId && ec.EncounterId == encounterId);
            return encounterCompletion != null ? true : false;
        }

        public bool HasUserCompletedEncounter(long userId, long encounterId)
        {
            var encounterCompletion = _dbSet.FirstOrDefault(ec => ec.UserId == userId && ec.EncounterId == encounterId && ec.Status == Core.Domain.Enums.EncounterCompletionStatus.COMPLETED);
            return encounterCompletion != null ? true : false;
        }

        public int GetTotalXPInDateRangeByUser(long userId, DateTime start, DateTime end)
        {
            var xpSum = _dbSet.Where(ec => userId == ec.UserId && ec.Status == Core.Domain.Enums.EncounterCompletionStatus.COMPLETED && ec.LastUpdatedAt > start && ec.LastUpdatedAt < end).Sum(ec => ec.Xp);
            return xpSum;
        }

        public int GetTotalXPInDateRangeByUsers(List<long> userIds, DateTime start, DateTime end)
        {
            var xpSum = _dbSet.Where(ec => userIds.Contains(ec.UserId) && ec.Status == Core.Domain.Enums.EncounterCompletionStatus.COMPLETED && ec.LastUpdatedAt > start && ec.LastUpdatedAt < end).Sum(ec => ec.Xp);
            return xpSum;
        }

        public List<EncounterCompletion> GetMembersCompletedHiddenEncounters(List<long> memberIds)
        {
            var encounterCompletions = _dbSet.Where(ec =>
                    memberIds.Contains(ec.UserId) && ec.Status == Core.Domain.Enums.EncounterCompletionStatus.COMPLETED)
                .ToList();
            return encounterCompletions;
        }

        public int GetCompletedCountByUser(long userId)
        {
            return _dbSet.Where(ec => ec.UserId == userId && ec.Status == Core.Domain.Enums.EncounterCompletionStatus.COMPLETED).Count();
        }

        public int GetFailedCountByUser(long userId)
        {
            return _dbSet.Where(ec => ec.UserId == userId && ec.Status == Core.Domain.Enums.EncounterCompletionStatus.FAILED).Count();
        }

        public int GetCompletedCountByUserAndMonth(long userId, int month, int year)
        {
            return _dbSet.Where(ec => ec.UserId == userId && ec.Status == Core.Domain.Enums.EncounterCompletionStatus.COMPLETED 
            && ec.LastUpdatedAt.Month == month && ec.LastUpdatedAt.Year == year).Count();
        }

        public int GetFailedCountByUserAndMonth(long userId, int month, int year)
        {
            return _dbSet.Where(ec => ec.UserId == userId && ec.Status == Core.Domain.Enums.EncounterCompletionStatus.FAILED
            && ec.LastUpdatedAt.Month == month && ec.LastUpdatedAt.Year == year).Count();
        }
    }
}
