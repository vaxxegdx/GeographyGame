using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GeographyGame.Backend.Data;
using GeographyGame.Backend.Models;

namespace GeographyGame.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly GeographyGameContext _db;

    public AuthController(GeographyGameContext db)
    {
        _db = db;
    }

    public struct LoginData
    {
        public string login { get; set; }
        public string password { get; set; }
    }

    [HttpPost]
    public IActionResult GetToken([FromBody] LoginData ld)
    {
        var user = _db.Users
            .FirstOrDefault(u => u.Login == ld.login);

        if (user == null || !user.CheckPassword(ld.password))
        {
            return Unauthorized(new { message = "wrong login/password" });
        }

        return Ok(AuthOptions.GenerateToken(user.IsAdmin));
    }
}