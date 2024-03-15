using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class ClubFightXPInfoDto
    {
        public List<FightParticipantInfoDto> Club1ParticipantsInfo { get; set; }
        public List<FightParticipantInfoDto> Club2ParticipantsInfo { get; set; }
        public int club1TotalXp { get; set; }
        public int club2TotalXp { get; set; }
    }
}
