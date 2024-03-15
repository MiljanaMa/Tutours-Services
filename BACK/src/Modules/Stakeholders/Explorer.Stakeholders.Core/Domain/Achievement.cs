using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Stakeholders.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Explorer.Stakeholders.Core.Domain
{
    public class Achievement: Entity
    {
        public String Name { get; init; }
        public String Description { get; init; }
        public AchievementType Type { get; init; }

        public Achievement() { }

        public Achievement(String name, String description, AchievementType type)
        {
            Name = name;
            Description = description;
            Type = type;
        }
    }
}
