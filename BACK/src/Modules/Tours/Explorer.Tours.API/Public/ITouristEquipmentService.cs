using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public;

public interface ITouristEquipmentService
{
    Result<TouristEquipmentDto> Create(TouristEquipmentDto equipment);
    Result Delete(TouristEquipmentDto equipment);
}