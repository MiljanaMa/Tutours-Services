namespace Explorer.Stakeholders.API.Dtos;

public class AccountRegistrationDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string ProfileImage { get; set; }
    public string Biography { get; set; }
    public string Quote { get; set; }
    public int? XP { get; set; }
    public int? Level { get; set; }
}