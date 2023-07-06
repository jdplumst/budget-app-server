using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace budget_app_server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly BudgetAppContext context;

    public UserController(BudgetAppContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public ActionResult<User> Get()
    {
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = context.Users.Where((u) => u.Id == userId).Select((u) => new { u.Username, u.Role }).FirstOrDefault();
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}