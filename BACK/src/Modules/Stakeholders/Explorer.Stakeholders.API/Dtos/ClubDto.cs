namespace Explorer.Stakeholders.API.Dtos;

public class ClubDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public long OwnerId { get; set; }
    public PersonDto? Owner { get; set; }
    public int? FightsWon { get; set; }
    public List<PersonDto>? Members { get; set; }
    public List<AchievementDto>? Achievements { get; set; }
}