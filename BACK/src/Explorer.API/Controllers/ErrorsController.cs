using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers;

[ApiController]
public class ErrorsController : ControllerBase
{
    [HttpGet]
    [Route("/error")]
    public IActionResult HandleErrors()
    {
        return Problem();
    }
}