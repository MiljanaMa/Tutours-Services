using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.Enums;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.Core.Domain;

using FluentResults;

namespace Explorer.Encounters.Core.UseCases
{
     public class KeypointEncounterService : CrudService<KeypointEncounterDto, KeypointEncounter>, IKeypointEncounterService
    {
        protected IKeypointEncounterRepository _keypointEncounterRepository;
        protected IEncounterRepository _encounterRepository;
        protected IInternalUserService _userService;
        protected IInternalProfileService _profileService;

        public KeypointEncounterService(IKeypointEncounterRepository keypointEncounterRepository, IEncounterRepository encounterRepository,
             IInternalUserService userService, IInternalProfileService profileService, IMapper mapper) : base(keypointEncounterRepository, mapper)
        {
            _keypointEncounterRepository = keypointEncounterRepository;
            _encounterRepository = encounterRepository;
            _userService = userService;
            _profileService = profileService;
        }
        public Result<PagedResult<KeypointEncounterDto>> GetPagedByKeypoint(int page, int pageSize, long keypointId)
        {
            var result = _keypointEncounterRepository.GetPagedByKeypoint(page, page, keypointId);
            return MapToDto(result);
        }
        public override Result<KeypointEncounterDto> Create(KeypointEncounterDto keypointEncounter)
        {
            try
            {
                keypointEncounter.Encounter.ApprovalStatus = EncounterApprovalStatus.SYSTEM_APPROVED.ToString();
                var result = _keypointEncounterRepository.Create(MapToDomain(keypointEncounter));
                return MapToDto(result);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result Delete(int keypointEncounterId)
        {
            try
            {
                var keypointEncounter = _keypointEncounterRepository.Get(keypointEncounterId);

                if(keypointEncounter == null) return Result.Fail(FailureCode.NotFound).WithError("Encounter is not find");
                
                _keypointEncounterRepository.Delete(keypointEncounterId);
                _encounterRepository.Delete(keypointEncounter.Encounter.Id);
                return Result.Ok();
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result UpdateEncountersLocation(LocationDto location, int keypointId)
        {
            try
            {
                var keypointEncounters = _keypointEncounterRepository.GetAllByKeypoint(keypointId);
                foreach (var keypointEncounter in keypointEncounters)
                {
                    keypointEncounter.Encounter.UpdateLocation(location.Latitude, location.Longitude);
                    _encounterRepository.Update(keypointEncounter.Encounter);
                }
                return Result.Ok();
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.Conflict).WithError(e.Message);
            }
        }
        public Result DeleteKeypointEncounters(int keypointId)
        {
            var keypointEncounters = _keypointEncounterRepository.GetAllByKeypoint(keypointId);
                foreach (var keypointEncounter in keypointEncounters)
                {
                    Delete((int)keypointEncounter.Id);
                }
                
            return Result.Ok();
        }
    }
}
