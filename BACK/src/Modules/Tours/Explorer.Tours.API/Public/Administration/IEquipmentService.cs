using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Administration;

public interface IEquipmentService
{
    Result<PagedResult<EquipmentDto>> GetPaged(int page, int pageSize);
    Result<EquipmentDto> Create(EquipmentDto equipment);
    Result<EquipmentDto> Update(EquipmentDto equipment);
    Result Delete(int id);
    IEnumerable<EquipmentForSelectionDto> GetAllForSelected(int userId);
}