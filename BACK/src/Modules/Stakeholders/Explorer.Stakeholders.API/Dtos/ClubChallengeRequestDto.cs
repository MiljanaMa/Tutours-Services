namespace Explorer.Stakeholders.API.Dtos;

public class ClubChallengeRequestDto
{
    public int? Id { get; set; }
    public ClubDto? Challenger { get; set; }
    public long ChallengerId { get; set; }
    public ClubDto? Challenged { get; set; }
    public long ChallengedId { get; set; }
    public string Status { get; set; }
}