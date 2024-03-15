namespace Explorer.Tours.API.Dtos;

public class TouristEquipmentDto
{
    public TouristEquipmentDto(long id, long touristId, long equipmentId)
    {
        Id = id;
        TouristId = touristId;
        EquipmentId = equipmentId;
    }

    public long Id { get; set; }
    public long TouristId { get; set; }
    public long EquipmentId { get; set; }
}