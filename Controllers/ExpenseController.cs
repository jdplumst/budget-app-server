using System.Security.Claims;
using BudgetApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budget_app_server.Controllers;

[ApiController, Authorize]
[Route("[controller]")]
public class ExpenseController : ControllerBase
{
  private readonly BudgetAppContext context;
  public ExpenseController(BudgetAppContext context)
  {
    this.context = context;
  }

  [HttpGet]
  public IActionResult GetAll(int projectId = 0)
  {
    int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

    if (projectId == 0)
    {
      return BadRequest();
    }
    else
    {
      var project = context.Projects.Find(projectId);
      if (project == null)
      {
        return NotFound("Project does not exist");
      }
      if (project.UserId != userId)
      {
        return Unauthorized("You are not authorized to view this project");
      }
      var expenses = context.Expenses.Where(e => e.ProjectId == projectId).ToList();
      return Ok(expenses);
    }
  }

  [HttpGet("{id}")]
  public IActionResult Get(int id)
  {
    var expense = context.Expenses.Find(id);
    if (expense == null)
    {
      return NotFound("Expense not found");
    }
    var project = context.Projects.Find(expense.ProjectId);
    if (project == null)
    {
      return NotFound("Project belonging to expense does not exist");
    }
    int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
    if (project.UserId != userId)
    {
      return Unauthorized("You are not authorized to view this expense");
    }
    return Ok(expense);
  }

  [HttpPost]
  public IActionResult Create(Expense expense)
  {
    if (string.IsNullOrWhiteSpace(expense.Name))
    {
      return BadRequest("Expense Name must be non-empty");
    }
    if (expense.Name.Length > 30)
    {
      return BadRequest("Expense Name must be 30 characters or less");
    }
    if (expense.Amount <= 0)
    {
      return BadRequest("Amount must be greater than $0");
    }
    int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
    var project = context.Projects.Find(expense.ProjectId);
    if (project == null)
    {
      return BadRequest("Project does not exist");
    }
    if (project.UserId != userId)
    {
      return Unauthorized("You are not authorized to create this expense");
    }
    if (!Enum.IsDefined(typeof(ExpenseType), expense.Type))
    {
      return BadRequest("Must give a valid Expense Type");
    }
    context.Expenses.Add(expense);
    Console.WriteLine(expense);
    context.SaveChanges();
    return CreatedAtAction(nameof(Get), new { id = expense.Id }, expense);
  }

  [HttpDelete("{id}")]
  public IActionResult Delete(int id)
  {
    int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
    var expense = context.Expenses.Find(id);
    if (expense == null)
    {
      return NotFound("Expense does not exist");
    }
    var project = context.Projects.Where(p => p.Id == expense.ProjectId).First();
    if (project.UserId != userId)
    {
      return Unauthorized("You are not authorized to delete this expense");
    }
    context.Expenses.Remove(expense);
    context.SaveChanges();
    return Ok(expense);
  }
}
