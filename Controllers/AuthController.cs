using BudgetApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Web;

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
    [Produces("application/json")]
    public ActionResult<String> Signup(UserDto userDto)
    {
        if (String.IsNullOrWhiteSpace(userDto.Username))
        {
            return BadRequest("Must enter a username");
        }
        if (String.IsNullOrWhiteSpace(userDto.Password))
        {
            return BadRequest("Must enter a password");
        }

        // Check if user with that username already exists
        var exists = context.Users.Where(u => u.Username.ToLower() == userDto.Username.ToLower()).FirstOrDefault();
        if (exists != null)
        {
            return BadRequest("Username already taken");
        }


        // Check Passowrd Strength
        if (userDto.Password.Length < 8 || !userDto.Password.Any((p) => char.IsLower(p))
            || !userDto.Password.Any((p) => char.IsUpper(p)) || !userDto.Password.Any((p) => char.IsDigit(p))
            || userDto.Password.IndexOfAny("\\|¬¦`!\"£$%^&*()_+-=[]{};:'@#~<>,./?".ToCharArray()) < 0)
        {
            return BadRequest(@"Password must contain at least 1 lowercase character, " +
                "1 uppercase character, 1 digit, 1 special character, and 8 characters total");
        }

        string username = userDto.Username;
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        User user = new User(username, passwordHash);
        context.Add<User>(user);
        context.SaveChanges();
        string token = CreateToken(user);
        HttpContext.Response.Cookies.Append("ba_session", token, new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1),
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.None,
            Domain = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "development"
                ? null : Environment.GetEnvironmentVariable("DOMAIN")
        });
        return Ok(token);
    }

    [HttpPost("login")]
    [Produces("application/json")]
    public ActionResult<String> Login(UserDto userDto)
    {
        if (String.IsNullOrWhiteSpace(userDto.Username))
        {
            return BadRequest("Must enter a username");
        }
        if (String.IsNullOrWhiteSpace(userDto.Password))
        {
            return BadRequest("Must enter a password");
        }

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
        HttpContext.Response.Cookies.Append("ba_session", token, new CookieOptions
        {
            Expires = DateTime.Now.AddDays(1),
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.None,
            Domain = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "development"
                ? null : Environment.GetEnvironmentVariable("DOMAIN")
        });
        return Ok(token);
    }

    private static string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(issuer: Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "development"
            ? Environment.GetEnvironmentVariable("ISSUERS")!.Split(" ")[0]
            : Environment.GetEnvironmentVariable("ISSUERS")!.Split(" ")[1],
            audience: Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "development"
            ? Environment.GetEnvironmentVariable("AUDIENCE")!.Split(" ")[0]
            : Environment.GetEnvironmentVariable("AUDIENCE")!.Split(" ")[1],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}