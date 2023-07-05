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
    public ActionResult<Project> Get(int id)
    {
        var project = context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }
        return project;
    }

    [HttpPost]
    public IActionResult Create(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.Name))
        {
            return BadRequest("Project Name bust be non-empty");
        }
        if (project.Budget <= 0)
        {
            return BadRequest("Budget must be greater than $0");
        }
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        project.UserId = userId;
        context.Projects.Add(project);
        context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Project project)
    {
        if (string.IsNullOrWhiteSpace(project.Name))
        {
            return BadRequest("Project Name must be non-empty");
        }
        if (project.Name.Length > 30)
        {
            return BadRequest("Project Name must be 30 characters or less");
        }
        if (project.Budget <= 0)
        {
            return BadRequest("Budget must be greater than $0");
        }
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var existing = context.Projects.Find(id);
        if (existing == null)
        {
            return NotFound("Project not found");
        }
        if (existing.UserId != userId)
        {
            return Unauthorized("You are not authorized to update this project");
        }
        existing.Name = project.Name;
        existing.Budget = project.Budget;
        context.SaveChanges();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var existing = context.Projects.Find(id);
        if (existing == null)
        {
            return NotFound("Project not found");
        }
        if (existing.UserId != userId)
        {
            return Unauthorized("You are not authorized to update this project");
        }
        context.Projects.Remove(existing);
        context.SaveChanges();
        return Ok(existing);
    }
}