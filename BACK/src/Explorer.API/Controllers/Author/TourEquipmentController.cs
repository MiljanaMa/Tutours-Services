using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author;

[Authorize(Policy = "userPolicy")]
[Route("api/author/tour-equipment")]
public class TourEquipmentController : BaseApiController
{
    private readonly ITourEquipmentService _tourEquipmentService;

    public TourEquipmentController(ITourEquipmentService tourEquipmentService)
    {
        _tourEquipmentService = tourEquipmentService;
    }

    [HttpPost("add")]
    public ActionResult<TourEquipmentDto> AddEquipmentToTour([FromBody] TourEquipmentDto tourEquipmentDto)
    {
        var result = _tourEquipmentService.AddEquipmentToTour(tourEquipmentDto);
        return Ok(result);
    }

    [HttpGet("{tourId:int}")]
    public ActionResult<List<EquipmentDto>> GetEquipmentForTour(int tourId)
    {
        var result = _tourEquipmentService.GetEquipmentForTour(tourId);
        return CreateResponse(result);
    }

    [HttpPost("remove")]
    public ActionResult RemoveEquipmentFromTour([FromBody] TourEquipmentDto tourEquipmentDto)
    {
        var result = _tourEquipmentService.RemoveEquipmentFromTour(tourEquipmentDto);
        return CreateResponse(result);
    }
}