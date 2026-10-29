using GeographyGame.Backend.Models;
using GeographyGame.Backend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeographyGame.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class MatchController : ControllerBase
{
    private readonly IGeographyGameRepository repository;

    public MatchController(IGeographyGameRepository context)
    {
        repository = context;
    }

    [HttpGet]
    public IActionResult GetMatchesList()
    {
        return Ok(repository.GetMatchesList());
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetMatch(int id)
    {
        var match = repository.GetMatch(id);

        if (match == null)
        {
            return NotFound();
        }

        return Ok(match);
    }

    [HttpPost("new")]
    [Authorize]
    public IActionResult NewMatch([FromBody] Match match)
    {
        repository.CreateMatch(match);
        repository.Save();

        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteMatch(int id)
    {
        if (repository.DeleteMatch(id))
        {
            repository.Save();
            return NoContent();
        }

        return NotFound();
    }

    [HttpPut("update/{id}")]
    [Authorize]
    public IActionResult UpdateMatch(int id, [FromBody] Match match)
    {
        if (id != match.Id)
        {
            return BadRequest();
        }

        if (repository.GetMatch(id) == null)
        {
            return NotFound();
        }

        repository.UpdateMatch(match);
        repository.Save();

        return NoContent();
    }
}