namespace Explorer.Tours.API.Dtos;

public class FilterCriteriaDto
{
    public double CurrentLatitude { get; set; }
    public double CurrentLongitude { get; set; }
    public double FilterRadius { get; set; }
}