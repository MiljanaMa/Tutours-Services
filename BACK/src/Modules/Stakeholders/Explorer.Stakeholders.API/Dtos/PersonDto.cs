namespace Explorer.Stakeholders.API.Dtos;

public class PersonDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string ProfileImage { get; set; }
    public string Biography { get; set; }
    public string Quote { get; set; }
    public string Email { get; set; }
    public int? XP { get; set; }
    public int? Level { get; set; }
    public ClubDto? Club { get; set; }
    public long? ClubId { get; set; }
}