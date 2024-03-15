namespace Explorer.Tours.API.Dtos;

public class PublicKeypointDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Description { get; set; }
    public int? Position { get; set; }
    public string? Image { get; set; }
}