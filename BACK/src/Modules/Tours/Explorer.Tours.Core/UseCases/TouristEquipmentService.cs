using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases;

public class TouristEquipmentService : CrudService<TouristEquipmentDto, TouristEquipment>, ITouristEquipmentService
{
    private readonly ICrudRepository<Equipment> _equipmentRepository;
    private readonly ITouristEquipmentRepository _touristEquipmentRepository;

    public TouristEquipmentService(ICrudRepository<Equipment> equipmentRepository,
        ITouristEquipmentRepository touristEquipmentRepository, ICrudRepository<TouristEquipment> repository,
        IMapper mapper) : base(repository, mapper)
    {
        _touristEquipmentRepository = touristEquipmentRepository;
        _equipmentRepository = equipmentRepository;
    }

    public Result<TouristEquipmentDto> Create(TouristEquipmentDto dto)
    {
        try
        {
            _equipmentRepository.Get(dto.EquipmentId);
        }
        catch (KeyNotFoundException ke)
        {
            return Result.Fail(FailureCode.InvalidArgument).WithError("");
        }

        var foundTouristEquipment =
            _touristEquipmentRepository.GetByTouristAndEquipment(dto.TouristId, dto.EquipmentId);
        if (foundTouristEquipment != null) return Result.Fail(FailureCode.InvalidArgument).WithError("");
        return base.Create(dto);
    }

    public Result Delete(TouristEquipmentDto dto)
    {
        try
        {
            _equipmentRepository.Get(dto.EquipmentId);
        }
        catch (KeyNotFoundException ke)
        {
            return Result.Fail(FailureCode.NotFound).WithError("");
        }

        var foundTouristEquipment =
            _touristEquipmentRepository.GetByTouristAndEquipment(dto.TouristId, dto.EquipmentId);
        if (foundTouristEquipment == null) return Result.Fail(FailureCode.NotFound).WithError("");
        return Delete((int)foundTouristEquipment.Id);
    }
}