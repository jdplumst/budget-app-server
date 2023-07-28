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

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var currUser = context.Users.Where(u => u.Id == userId).FirstOrDefault();
        if (currUser == null || (userId != id && !currUser.Role.HasFlag(Role.Admin)))
        {
            return Unauthorized("You are not authorized to delete this user");
        }
        var user = context.Users.Where(u => u.Id == id).FirstOrDefault();
        if (user == null)
        {
            return NotFound("User does not exist");
        }
        context.Users.Remove(user);
        context.SaveChanges();
        return Ok();
    }
}