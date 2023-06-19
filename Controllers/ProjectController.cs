using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace budget_app_server.Controllers;

[ApiController, Authorize]
[Route("[controller]")]
public class ProjectController : ControllerBase
{

    private readonly BudgetAppContext context;

    public ProjectController(BudgetAppContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public ActionResult<List<Project>> GetAll()
    {
        // var user = User.Identity?.Name;
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        // int id = Convert.ToInt32(HttpContext.User.FindFirstValue("userID"));
        return context.Projects.Where(p => p.UserId == userId).ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Project> GetById(int id)
    {
        var project = context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }
        return project;
    }
}