using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration;

[Route("api/administration/users")]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "administrator")]
    public ActionResult<PagedResult<UserDto>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _userService.GetPaged(page, pageSize);
        return CreateResponse(result);
    }

    [HttpPost]
    [Authorize(Roles = "administrator")]
    public ActionResult<UserDto> Create([FromBody] UserDto user)
    {
        var result = _userService.Create(user);
        return CreateResponse(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "administrator")]
    public ActionResult<UserDto> Update([FromBody] UserDto user)
    {
        var result = _userService.Update(user);
        return CreateResponse(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "administrator")]
    public ActionResult Delete(int id)
    {
        var result = _userService.Delete(id);
        return CreateResponse(result);
    }
    
    [HttpGet("allTourists")]
    [Authorize(Roles = "author")]
    public ActionResult<PagedResult<UserDto>> GetAllTourists([FromQuery] int page, [FromQuery] int pageSize)
    {
        var result = _userService.GetAllTourists(page, pageSize);
        return CreateResponse(result);

    }

    [HttpGet("verify")]
    [AllowAnonymous]
    public ActionResult Verify(string token)
    {
        var user = _userService.GetByToken(token).Value;
        user.IsEnabled = true;
        var result = _userService.Update(user);
        if (result.IsSuccess)
        {
            return Ok("Confirmed");
        }
        return CreateResponse(result);
    }
}