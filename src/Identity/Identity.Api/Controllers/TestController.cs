using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[AllowAnonymous]
public class TestController : Controller
{
    public IActionResult Index()
    {
        return Ok("it's working");
    }
}
