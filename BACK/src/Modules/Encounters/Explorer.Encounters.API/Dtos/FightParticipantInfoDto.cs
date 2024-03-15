using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class FightParticipantInfoDto
    {
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public int? Level { get; set; }
        public int XPInFight { get; set; }
    }
}
