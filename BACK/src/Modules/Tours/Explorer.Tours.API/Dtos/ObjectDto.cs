using Explorer.Tours.API.Dtos.Enums;

namespace Explorer.Tours.API.Dtos;

public class ObjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Category { get; set; }
    public ObjectStatus Status { get; set; }
}