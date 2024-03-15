using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist;

[Authorize(Policy = "touristPolicy")]
[Route("api/tourist/touristEquipment")]
public class TouristEquipmentController : BaseApiController
{
    private readonly IEquipmentService _equipmentService;
    private readonly ITouristEquipmentService _touristEquipmentService;

    public TouristEquipmentController(ITouristEquipmentService touristEquipmentService,
        IEquipmentService equipmentService)
    {
        _touristEquipmentService = touristEquipmentService;
        _equipmentService = equipmentService;
    }

    [HttpGet("forSelected/{id:int}")]
    public ActionResult<IEnumerable<EquipmentForSelectionDto>> GetAllForSelected(int id)
    {
        var result = _equipmentService.GetAllForSelected(id);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<TouristEquipmentDto> Create([FromBody] TouristEquipmentDto touristEquipment)
    {
        var result = _touristEquipmentService.Create(touristEquipment);
        return CreateResponse(result);
    }

    [HttpPost("deleteByTouristAndEquipmentId")]
    public ActionResult Delete([FromBody] TouristEquipmentDto touristEquipment)
    {
        var result = _touristEquipmentService.Delete(touristEquipment);
        return CreateResponse(result);
    }
}