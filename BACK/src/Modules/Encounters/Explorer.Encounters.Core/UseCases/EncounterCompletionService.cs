using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Encounters.Core.Domain;
using Explorer.Encounters.Core.Domain.Enums;
using Explorer.Encounters.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Internal;
using FluentResults;
using System;

namespace Explorer.Encounters.Core.UseCases
{
    public class EncounterCompletionService : CrudService<EncounterCompletionDto, EncounterCompletion>, IEncounterCompletionService
    {
        protected IEncounterCompletionRepository _encounterCompletionRepository;
        protected IEncounterRepository _encounterRepository;
        protected IInternalTouristPositionService _touristPositionService;
        protected IInternalProfileService _profileService;
        protected IInternalAchievementService _achievementService;
        protected IInternalClubService _clubService;

        private const double HiddenLocationRange = 0.50; // was 0.050
        private const double HiddenLocationInterval = 5;

        public EncounterCompletionService(IEncounterCompletionRepository encoutnerCompletionRepository, IInternalTouristPositionService touristPositionService,
            IEncounterRepository encounterRepository, IInternalProfileService profileService,
            IInternalAchievementService achievementService, IInternalClubService clubService, IMapper mapper) : base(encoutnerCompletionRepository, mapper)
        {
            _encounterCompletionRepository = encoutnerCompletionRepository;
            _touristPositionService = touristPositionService;
            _encounterRepository = encounterRepository;
            _profileService = profileService;
            _achievementService = achievementService;
            _clubService = clubService;
        }

        public Result<PagedResult<EncounterCompletionDto>> GetPagedByUser(int page, int pageSize, int userId)
        {
            var result = _encounterCompletionRepository.GetPagedByUser(page, page, userId);
            return MapToDto(result);
        }

        public EncounterCompletionDto GetByUserAndEncounter(int userId, int encounterId)
        {
            var result = _encounterCompletionRepository.GetByUserAndEncounter(userId, encounterId);
            return MapToDto(result);
        }

        public Result<List<EncounterCompletionDto>> GetByIds(List<int> ids)
        {
            List<EncounterCompletion> results = new List<EncounterCompletion>();
            foreach (var id in ids)
            {
                try
                {
                    var result = _encounterCompletionRepository.GetByEncounter(id); // can be moved to Repo probably, so we don't have to do foreach here
                    if (result == null) continue;
                    results.Add(result);
                }
                catch(KeyNotFoundException e)
                {
                    continue;
                }
            }
            return MapToDto(results);
        }

        public void UpdateSocialEncounters()
        {
            List<Encounter> socialEncounters = _encounterRepository.GetApprovedByStatusAndType(EncounterStatus.ACTIVE, EncounterType.SOCIAL).ToList();
            List<TouristPositionDto> touristPositions = _touristPositionService.GetPaged(0, 0).ValueOrDefault.Results;
            List<long> nearbyUserIds = new List<long>();

            foreach (var encounter in socialEncounters)
            {
                nearbyUserIds = touristPositions
                        .Where(position => IsTouristInRangeAndUpdated(position, encounter))
                        .Select(position => position.UserId)
                        .Distinct()
                        .ToList();

                foreach (long userId in nearbyUserIds)
                {
                    if (!_encounterCompletionRepository.HasUserStartedEncounter(userId, encounter.Id))
                    {
                        EncounterCompletion encounterCompletion = new EncounterCompletion(userId, encounter.Id, encounter.Xp, EncounterCompletionStatus.STARTED);
                        _encounterCompletionRepository.Create(encounterCompletion);
                    }

                    if (nearbyUserIds.Count >= encounter.PeopleCount)
                    {
                        if (!_encounterCompletionRepository.HasUserCompletedEncounter(userId, encounter.Id))
                        {
                            EncounterCompletion encounterCompletion = _encounterCompletionRepository.GetByUserAndEncounter(userId, encounter.Id);
                            encounterCompletion.UpdateStatus(EncounterCompletionStatus.COMPLETED);
                            _encounterCompletionRepository.Update(encounterCompletion);
                            _profileService.AddXP((int)userId, encounter.Xp);

                        }
                    }
                }
            }
        }
        public Result<EncounterCompletionDto> StartEncounter(long userId, EncounterDto encounter)
        {
            Result<TouristPositionDto> position = _touristPositionService.GetByUser(userId);

            if(position.IsSuccess == false) return Result.Fail(FailureCode.NotFound).WithError("Set your position first");

            EncounterCompletion encounterCompletion = new EncounterCompletion(userId, encounter.Id, encounter.Xp, EncounterCompletionStatus.STARTED);
            if (!_encounterCompletionRepository.HasUserStartedEncounter(userId, encounter.Id) && IsTouristInRange(position.Value, encounter.Longitude, encounter.Latitude, encounter.Range))
            {
                try
                {
                    var result = _encounterCompletionRepository.Create(encounterCompletion);
                    return Result.Ok(MapToDto(result));
                }
                catch (KeyNotFoundException e)
                {
                    return Result.Fail(FailureCode.Conflict).WithError(e.Message);
                }
            }
            return Result.Fail(FailureCode.Conflict).WithError("This encounter can't be started");

        }
        public Result<EncounterCompletionDto> FinishEncounter(long userId, EncounterDto encounter)
        {
            var encounterCompletion = _encounterCompletionRepository.GetByUserAndEncounter(userId, encounter.Id);
            if (encounterCompletion == null) return Result.Fail(FailureCode.NotFound).WithError("You didn't started encounter yet");

            encounterCompletion.UpdateStatus(EncounterCompletionStatus.COMPLETED);
            var result = _encounterCompletionRepository.Update(encounterCompletion);
            _profileService.AddXP((int)userId, encounter.Xp);

            return Result.Ok(MapToDto(result));

        }

        public Result<List<EncounterCompletionDto>> CheckNearbyEncounters(int userId)
        {
            TouristPositionDto touristPosition = _touristPositionService.GetByUser(userId).ValueOrDefault;
            PersonDto tourist = _profileService.GetFull(userId).Value;
            if(touristPosition == null)
            {
                return Result.Fail(FailureCode.NotFound).WithError("You don't have set position");
            }
            List<Encounter> nearbyEncounters = _encounterRepository.GetNearbyByType(0, 0, touristPosition.Longitude, touristPosition.Latitude, EncounterType.LOCATION).Results.ToList();
            List<EncounterCompletion> completedEncounters = new List<EncounterCompletion>();

            foreach (var encounter in nearbyEncounters)
            {
                EncounterCompletion encounterCompletion = _encounterCompletionRepository.GetByUserAndEncounter(userId, encounter.Id);
                if (encounterCompletion == null || encounterCompletion.IsFinished) continue;

                if (DistanceCalculator.CalculateDistance((double)encounter.ImageLatitude, (double)encounter.ImageLongitude,
                    touristPosition.Latitude, touristPosition.Longitude) > HiddenLocationRange)
                {
                    encounterCompletion.Reset();
                    _encounterCompletionRepository.Update(encounterCompletion);
                    continue;
                }
                else if (encounterCompletion.IsStarted)
                {
                    encounterCompletion.Progress();
                    _encounterCompletionRepository.Update(encounterCompletion);
                }

                if ((DateTime.UtcNow - encounterCompletion.LastUpdatedAt).TotalSeconds >= HiddenLocationInterval)
                {
                    encounterCompletion.Complete();
                    _profileService.AddXP((int)userId, encounterCompletion.Xp);
                    _encounterCompletionRepository.Update(encounterCompletion);
                    completedEncounters.Add(encounterCompletion);

                    if (tourist.Club == null || tourist.Club.Achievements.Any(a => (int)a.Type == 0))
                        break;
                    List<long> memberIds = tourist.Club.Members.ConvertAll(member => member.Id);
                    List <EncounterCompletion> completions = _encounterCompletionRepository.GetMembersCompletedHiddenEncounters(memberIds);
                    AchievementDto achievement = _achievementService.getHiddenEncounterAchievement(completions.Count);
                    
                    if (achievement != null)
                        _clubService.AddAchievement((long)tourist.ClubId, achievement.Id);

                }
            }

            return MapToDto(completedEncounters);
        }
        private bool IsTouristInRangeAndUpdated(TouristPositionDto position, Encounter encounter)
        {
            bool isInRange = IsTouristInRange(position, encounter.Longitude, encounter.Latitude, encounter.Range);
            bool updatedRecently = position.UpdatedAt > DateTime.UtcNow.AddMinutes(-10);

            return isInRange && updatedRecently;
        }

        private bool IsTouristInRange(TouristPositionDto position, double longitude, double latitude, double range)
        {
            double touristDistance = DistanceCalculator.CalculateDistance(position.Latitude, position.Longitude, latitude, longitude);
            bool isInRange = touristDistance < range;
            return isInRange;
        }
    }
}
