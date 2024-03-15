using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Encounters.Core.Domain
{
    public class KeypointEncounter: Entity
    {
        public long EncounterId { get; init; }
        public Encounter Encounter { get; init; }
        public long KeyPointId { get; init; }
        public bool IsRequired { get; init; }
        public KeypointEncounter()
        {

        }

        public KeypointEncounter(long encounterId, long keypointId, bool isRequired)
        {
            EncounterId = encounterId;
            KeyPointId = keypointId;
            IsRequired = isRequired;
        }

    }
}
