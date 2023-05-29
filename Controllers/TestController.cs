using Microsoft.AspNetCore.Mvc;

namespace budget_app_server.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    public TestController() { }

    [HttpGet]
    public ActionResult<String> GetAll()
    {
        var testvar = Environment.GetEnvironmentVariable("THIS");
        return "this " + testvar;
    }
}