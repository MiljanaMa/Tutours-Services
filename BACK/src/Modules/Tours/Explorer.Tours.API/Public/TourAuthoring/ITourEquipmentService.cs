using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.TourAuthoring;

public interface ITourEquipmentService
{
    Result<List<EquipmentDto>> GetEquipmentForTour(int tourId);
    Result AddEquipmentToTour(TourEquipmentDto tourEquipmentDto);
    Result RemoveEquipmentFromTour(TourEquipmentDto tourEquipmentDto);
}