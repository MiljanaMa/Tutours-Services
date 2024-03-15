using AutoMapper;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.Core.Domain;

namespace Explorer.Encounters.Core.Mappers
{
    public class EncounterProfile : Profile
    {
        public EncounterProfile()
        {
            CreateMap<EncounterDto, Encounter>().ReverseMap();
            CreateMap<EncounterCompletionDto, EncounterCompletion>().ReverseMap();
            CreateMap<KeypointEncounterDto, KeypointEncounter>().ReverseMap();
        }
    }
}
