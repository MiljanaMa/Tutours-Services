using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Enums;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class AchievementDatabaseRepository : CrudDatabaseRepository<Achievement, StakeholdersContext>, IAchievementRepository
    {
        protected readonly StakeholdersContext DbContext;

        public AchievementDatabaseRepository(StakeholdersContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public Achievement GetNoTracking(int id)
        {
            return DbContext.Achievements.AsNoTracking().FirstOrDefault(a => a.Id == id);
        }

        public Achievement GetByType(AchievementType type)
        {
            return DbContext.Achievements.AsNoTracking().FirstOrDefault(achievement => achievement.Type == type);
        }
    }
}
