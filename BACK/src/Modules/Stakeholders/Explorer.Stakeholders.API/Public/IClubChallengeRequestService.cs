using Explorer.Stakeholders.API.Dtos;
using FluentResults;

namespace Explorer.Stakeholders.API.Public;

public interface IClubChallengeRequestService
{
    Result<ClubChallengeRequestDto> Create(ClubChallengeRequestDto request);
    Result<ClubChallengeRequestDto> Update(ClubChallengeRequestDto request);
    Result<ClubChallengeRequestDto> AcceptChallenge(ClubChallengeRequestDto request);
    Result<List<ClubChallengeRequestDto>> GetCurrentChallengesForClub(long clubId);
}