using Microsoft.AspNetCore.Mvc;

namespace Example.API.Controllers;

[ApiController]
public class HelloWorldController : ControllerBase
{
    [HttpGet("hello")]
    public IActionResult HelloWorld()
    {
        var name = Environment.GetEnvironmentVariable("NAME");
        return Ok(string.IsNullOrEmpty(name) ? "Hello from fl0!" : $"Hello from fl0, {name}!");
    }
}
