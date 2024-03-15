using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Internal;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases.Identity;

public class ProfileService : CrudService<PersonDto, Person>, IProfileService, IInternalProfileService
{
    private readonly IMapper _mapper;
    private readonly IPersonRepository _personRepository;

    public ProfileService(IPersonRepository personRepository, IMapper mapper) : base(personRepository, mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public Result<List<PersonDto>> GetMany(List<int> peopleIds)
    {
        List<PersonDto> people = new();
        foreach (var id in peopleIds)
        {
            var person = _personRepository.Get(id);
            if (person != null)
            {
                var newPerson = _mapper.Map<PersonDto>(person);
                people.Add(newPerson);
            }
        }

        return people;
    }

    public void AddXP(int userId, int addedXp)
    {
        var person = _personRepository.Get(userId);
        if (person != null)
        {
            person.AddXp(addedXp);
            _personRepository.Update(person);
        }
    }


    public Result<PagedResult<PersonDto>> GetUserNonFollowedProfiles(int page, int pageSize, long userId)
    {
        try
        {
            var profiles = _personRepository.GetPaged(page, pageSize).Results;
            var user = _personRepository.GetFullProfile(userId);

            var nonFollowedProfiles =
                profiles.Where(profile => !user.Following.Contains(profile) && profile.Id != user.Id).ToList();
            var results = new PagedResult<Person>(nonFollowedProfiles, nonFollowedProfiles.Count);
            return MapToDto(results);
        }
        catch (KeyNotFoundException e)
        {
            return Result.Fail(FailureCode.NotFound).WithError(e.Message);
        }
    }

    public Result<AccountRegistrationDto> GetProfile(long userId)
    {
        var personProfile = _personRepository.Get(userId);


        var account = new AccountRegistrationDto
        {
            Name = personProfile.Name,
            Surname = personProfile.Surname,
            ProfileImage = personProfile.ProfileImage,
            Biography = personProfile.Biography,
            Quote = personProfile.Quote,
            XP = personProfile.XP,
            Level = personProfile.Level
        };

        return Result.Ok(account);
    }

    public Result<PersonDto> GetPersonDto(long userId)
    {
        var personProfile = _personRepository.Get(userId);
        
        var endMySuffering = new PersonDto()
        {
            Name = personProfile.Name,
            Surname = personProfile.Surname,
            ProfileImage = personProfile.ProfileImage,
            Biography = personProfile.Biography,
            Quote = personProfile.Quote,
            XP = personProfile.XP,
            Level = personProfile.Level,
            ClubId = personProfile.ClubId
        };

        return endMySuffering;
    }

    public Result<PersonDto> UpdateProfile(PersonDto updatedPerson)
    {
        var existingPerson = _personRepository.Get(updatedPerson.Id);

        if (existingPerson == null) return Result.Fail(FailureCode.NotFound);

        _mapper.Map(updatedPerson, existingPerson);
        _personRepository.Update(existingPerson);

        var updatedPersonDto = _mapper.Map<PersonDto>(existingPerson);

        return Result.Ok(updatedPersonDto);
    }

    public Result<PagedResult<PersonDto>> GetFollowers(long userId)
    {
        var userProfile = _personRepository.GetFullProfile(userId);
        var results = new PagedResult<Person>(userProfile.Followers, userProfile.Followers.Count);
        return MapToDto(results);
    }

    public Result<PagedResult<PersonDto>> GetFollowing(long userId)
    {
        var userProfile = _personRepository.GetFullProfile(userId);
        var results = new PagedResult<Person>(userProfile.Following, userProfile.Following.Count);
        return MapToDto(results);
    }

    public Result<PagedResult<PersonDto>> Follow(long followerId, long followedId)
    {
        var follower = _personRepository.GetFullProfile(followerId);
        var followed = _personRepository.Get(followedId);

        follower.AddFollowing(followed);
        _personRepository.Update(follower);

        var results = new PagedResult<Person>(follower.Following, follower.Following.Count);
        return MapToDto(results);
    }

    public Result<PagedResult<PersonDto>> Unfollow(long followerId, long unfollowedId)
    {
        var follower = _personRepository.GetFullProfile(followerId);
        var unfollowed = _personRepository.Get(unfollowedId);

        follower.RemoveFollowing(unfollowed);
        _personRepository.Update(follower);

        var results = new PagedResult<Person>(follower.Following, follower.Following.Count);
        return MapToDto(results);
    }

    public Result<bool> CanTouristCreateEncounters(long touristId) 
    {
        var tourist = _personRepository.GetFullProfile(touristId);
        return tourist.CanTouristCreateEncounters();
    }
    public Result<PersonDto> Get(long userId)
    {
        return MapToDto(_personRepository.Get(userId));
    }
    public Result<PersonDto> GetFull(long userId)
    {
        return MapToDto(_personRepository.GetFull(userId));
    }
}