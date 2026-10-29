using GeographyGame.Backend.Data;
using GeographyGame.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GeographyGame.Backend.Repository;

namespace GeographyGame.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IGeographyGameRepository repository;

    public UserController(IGeographyGameRepository context)
    {
        repository = context;
    }

    [HttpGet]
    public IActionResult GetUsersList()
    {
        return Ok(repository.GetUsersList());
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetUser(int id)
    {
        return Ok(repository.GetUser(id));
    }

    [HttpPost("new")]
    [Authorize(Roles = "admin")]
    public IActionResult NewUser([FromBody] User user)
    {
        repository.CreateUser(user);
        repository.Save();
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteUser(int id)
    {
        switch (repository.DeleteUser(id))
        {
            case true:
                repository.Save();
                return NoContent();
            case false:
                return NotFound();
            default:
        }
    }

    [HttpPut("update/{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult UpdateUser(int id, [FromBody] User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        repository.UpdateUser(user);
        repository.Save();
        return NoContent();
    }
}