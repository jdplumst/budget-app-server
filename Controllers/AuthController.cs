using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

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
        if (String.IsNullOrWhiteSpace(userDto.Username))
        {
            return BadRequest("Must enter a username");
        }
        if (String.IsNullOrWhiteSpace(userDto.Password))
        {
            return BadRequest("Must enter a password");
        }

        string username = userDto.Username;
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        var user = new User { Username = username, PasswordHash = passwordHash };
        context.Add<User>(user);
        context.SaveChanges();
        return Ok(user);
    }

    [HttpPost("login")]
    public ActionResult<String> Login(UserDto userDto)
    {
        var user = context.Users.Where(u => u.Username == userDto.Username).FirstOrDefault();
        if (user == null)
        {
            return BadRequest("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
        {
            return BadRequest("Incorrect password");
        }

        string token = CreateToken(user);

        return Ok(token);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
        new Claim(ClaimTypes.Name, user.Username)
      };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}