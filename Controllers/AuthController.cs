using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace budget_app_server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly BudgetAppContext context;

    public AuthController(BudgetAppContext context)
    {
        this.context = context;
    }

    [HttpPost("signup")]
    public ActionResult<User> Signup(UserDto userDto)
    {
        string username = userDto.Username;
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        var user = new User { Username = username, PasswordHash = passwordHash };
        context.Add<User>(user);
        context.SaveChanges();
        return Ok(user);
    }

    // [HttpGet]
    // public ActionResult<List<Project>> GetAll()
    // {
    //     return context.Projects.ToList(); ;
    // }

    // [HttpGet("{id}")]
    // public ActionResult<Project> GetById(int id)
    // {
    //     var project = context.Projects.Find(id);
    //     if (project == null)
    //     {
    //         return NotFound();
    //     }
    //     return project;
    // }
}