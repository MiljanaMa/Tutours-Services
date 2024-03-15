namespace Explorer.Stakeholders.API.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public bool IsActive { get; set; }
    public string Email { get; set; }
    public bool IsBlocked { get; set; }
    public bool IsEnabled {  get; set; }
    public string VerificationToken { get; set; }
}