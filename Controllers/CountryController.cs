using System.Diagnostics.Contracts;
using GeographyGame.Backend.Models;
using GeographyGame.Backend.Data;
using Microsoft.AspNetCore.Mvc;
using GeographyGame;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GeographyGame.Backend.Repository;
using System.Reflection.Metadata.Ecma335;

namespace GeographyGame.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CountryController : ControllerBase
{
    private readonly IGeographyGameRepository repository;

    public CountryController(IGeographyGameRepository context)
    {
        repository = context;
    }

    [HttpGet]
    public IActionResult GetCountriesList()
    {
        return Ok(repository.GetCountriesList());
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetCountry(int id)
    {
        return Ok(repository.GetCountry(id));
    }

    [HttpPost("new")]
    [Authorize(Roles = "admin")]
    public IActionResult NewCountry([FromBody] Country country)
    {
        repository.CreateCountry(country);
        repository.Save();
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteCountry(int id)
    {
        var result = repository.DeleteCountry(id);

        switch (result)
        {
            case DeleteCountryResult.NotFound:
                return NotFound();

            case DeleteCountryResult.HasPhotos:
                return Conflict();

            case DeleteCountryResult.Deleted:
                repository.Save();
                return NoContent();

            default:
                return StatusCode(500);
        }
    }

    [HttpPut("update/{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult UpdateCountry(int id, [FromBody] Country updatedCountry)
    {
        if (id != updatedCountry.Id)
        {
            return BadRequest();
        }

        repository.UpdateCountry(updatedCountry);
        repository.Save();
        return NoContent();
    }
}