using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.TourAuthoring;

public class TourEquipmentService : BaseService<TourEquipmentDto, TourEquipment>, ITourEquipmentService
{
    private readonly IMapper _mapper;
    protected readonly ITourEquipmentRepository _tourEquipmentRepository;

    public TourEquipmentService(ITourEquipmentRepository tourEquipmentRepository, IMapper mapper) : base(mapper)
    {
        _tourEquipmentRepository = tourEquipmentRepository;
        _mapper = mapper;
    }

    public Result<List<EquipmentDto>> GetEquipmentForTour(int tourId)
    {
        var equipment = _tourEquipmentRepository.GetEquipmentForTour(tourId);
        var equipmentDto = _mapper.Map<List<EquipmentDto>>(equipment);
        return Result.Ok(equipmentDto);
    }

    public Result AddEquipmentToTour(TourEquipmentDto tourEquipmentDto)
    {
        var tourEquipment = MapToDomain(tourEquipmentDto);
        _tourEquipmentRepository.AddEquipmentToTour(tourEquipment);
        return Result.Ok();
    }

    public Result RemoveEquipmentFromTour(TourEquipmentDto tourEquipmentDto)
    {
        var tourEquipment = MapToDomain(tourEquipmentDto);
        _tourEquipmentRepository.RemoveEquipmentFromTour(tourEquipment);
        return Result.Ok();
    }
}