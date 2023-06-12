using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
        return context.Projects.ToList(); ;
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