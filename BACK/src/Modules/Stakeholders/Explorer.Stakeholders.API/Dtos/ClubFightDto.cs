namespace Explorer.Stakeholders.API.Dtos;

public class ClubFightDto
{
    public int? Id { get; set; }
    public int? WinnerId { get; set; }
    public DateTime StartOfFight { get; set; }
    public DateTime EndOfFight { get; set; }
    public int Club1Id { get; set; }
    public int Club2Id { get; set; }
    public ClubDto? Club1 { get; set; }
    public ClubDto? Club2 { get; set; }
    public bool IsInProgress { get; set; }
}