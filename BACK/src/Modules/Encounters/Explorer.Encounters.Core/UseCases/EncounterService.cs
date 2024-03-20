using System.Text.Json;
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
using Explorer.Tours.API.Internal;
using Explorer.Tours.API.Dtos;
using FluentResults;


namespace Explorer.Encounters.Core.UseCases
{
    public class EncounterService: CrudService<EncounterDto, Encounter>, IEncounterService
    {
        protected IEncounterRepository _encounterRepository;
        protected IInternalUserService _userService;
        protected IInternalProfileService _profileService;
        protected IInternalTouristPositionService _touristPositionService;

        public EncounterService(IEncounterRepository encounterRepository,
            IInternalUserService userService, IInternalProfileService profileService,
            IInternalTouristPositionService touristPositionService, IMapper mapper) : base(encounterRepository, mapper)
        {
            _encounterRepository = encounterRepository;
            _userService = userService;
            _profileService = profileService;
            _touristPositionService = touristPositionService;
        }

        public Result<PagedResult<EncounterDto>> GetApproved(int page, int pageSize)
        {
            var result = _encounterRepository.GetApproved(page, pageSize);
            return MapToDto(result);
        }

        public Result<PagedResult<EncounterDto>> GetApprovedByStatus(int page, int pageSize, string status)
        {
            if (Enum.TryParse<EncounterStatus>(status, out var encounterStatus))
            {
                var encounters = _encounterRepository.GetApprovedByStatus(page, pageSize, encounterStatus);
                return MapToDto(encounters);
            }
            else
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("Invalid status value");
            }
        }

        public Result<PagedResult<EncounterDto>> GetByUser(int page, int pageSize, long userId)
        {
            var result = _encounterRepository.GetByUser(page, pageSize, userId);
            return MapToDto(result);
        }

        public Result<PagedResult<EncounterDto>> GetTouristCreatedEncounters(int page, int pageSize)
        {
            var result = _encounterRepository.GetTouristCreatedEncounters(page, pageSize);
            return MapToDto(result);
        }

        public override Result<EncounterDto> Create(EncounterDto encounterDto)
        {
            try
            {
                UserDto user = _userService.Get(encounterDto.UserId).ValueOrDefault;
                if (user.Role == (int)UserRole.Tourist)
                {                 
                    if (_profileService.CanTouristCreateEncounters(encounterDto.UserId).Value) 
                    {
                        var encounter = MapToDomain(encounterDto);
                        encounter.UpdateApprovalStatus(EncounterApprovalStatus.PENDING);

                        var jsonEncounter = JsonSerializer.Serialize(encounterDto);

                        string url = "localhost:8083/encounters";
                        HttpClient client = new HttpClient();

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(
                            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = client.PostAsync("http://localhost:8083/encounters",
                            new StringContent(jsonEncounter, System.Text.Encoding.UTF8, "application/json")).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            string json = response.Content.ReadAsStringAsync().Result;
                            EncounterDto result = JsonSerializer.Deserialize<EncounterDto>(json);

                            return Result.Ok(result);

                        }

                        return Result.Fail<EncounterDto>(response.ReasonPhrase); 

                    }
                    else
                    {
                        return Result.Fail("Encounter can not be created.");
                    }
                }
                else
                {
                    var encounter = MapToDomain(encounterDto);
                    encounter.UpdateApprovalStatus(EncounterApprovalStatus.PENDING);

                    var jsonEncounter = JsonSerializer.Serialize(encounterDto);

                    string url = "localhost:8083/encounters";
                    HttpClient client = new HttpClient();

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.PostAsync("http://localhost:8083/encounters",
                        new StringContent(jsonEncounter, System.Text.Encoding.UTF8, "application/json")).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string json = response.Content.ReadAsStringAsync().Result;
                        EncounterDto result = JsonSerializer.Deserialize<EncounterDto>(json);

                        return Result.Ok(result);

                    }

                    return Result.Fail<EncounterDto>(response.ReasonPhrase); 
                }               
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<EncounterDto> Approve(EncounterDto encounterDto)
        {
            try
            {
                var encounter = MapToDomain(encounterDto);
                encounter.UpdateApprovalStatus(EncounterApprovalStatus.ADMIN_APPROVED);
                var result = _encounterRepository.Update(encounter);
                return MapToDto(result);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<EncounterDto> Decline(EncounterDto encounterDto)
        {
            try
            {
                var encounter = MapToDomain(encounterDto);
                encounter.UpdateApprovalStatus(EncounterApprovalStatus.DECLINED);
                var result = _encounterRepository.Update(encounter);
                return MapToDto(result);
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }


        public Result<PagedResult<EncounterDto>> GetNearbyHidden(int page, int pageSize, int userId)
        {
            try {
                TouristPositionDto touristPosition = _touristPositionService.GetByUser(userId).Value;
                var encounters = _encounterRepository.GetNearbyByType(page, pageSize, touristPosition.Longitude, touristPosition.Latitude, EncounterType.LOCATION);
                return MapToDto(encounters);

            }catch(Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
        public Result<PagedResult<EncounterDto>> GetNearby(int page, int pageSize, int userId)
        {
            try {
                TouristPositionDto touristPosition = _touristPositionService.GetByUser(userId).Value;
                var encounters = _encounterRepository.GetNearby(page, pageSize, touristPosition.Longitude, touristPosition.Latitude);
                return MapToDto(encounters);

            }catch(Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
    }
}
