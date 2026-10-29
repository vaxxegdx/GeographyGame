using GeographyGame.Backend.Data;
using GeographyGame.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GeographyGame.Backend.Repository;

[ApiController]
[Route("[controller]")]
public class TravelPhotoController : ControllerBase
{
    private readonly IGeographyGameRepository repository;

    public TravelPhotoController(IGeographyGameRepository context)
    {
        repository = context;
    }

    [HttpGet]
    public IActionResult GetTravelPhotosList()
    {
        return Ok(repository.GetTravelPhotosList());
    }

    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetTravelPhoto(int id)
    {
        var photo = repository.GetTravelPhoto(id);

        if (photo == null)
        {
            return NotFound();
        }

        return Ok(photo);
    }

    [HttpPost("new")]
    [Authorize(Roles = "admin")]
    public IActionResult NewTravelPhoto([FromBody] TravelPhoto photo)
    {
        repository.CreateTravelPhoto(photo);
        repository.Save();
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteTravelPhoto(int id)
    {
        switch (repository.DeleteTravelPhoto(id))
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
    public IActionResult UpdateTravelPhoto(int id, [FromBody] TravelPhoto updatedPhoto)
    {
        if (id != updatedPhoto.Id)
        {
            return BadRequest();
        }

        repository.UpdateTravelPhoto(updatedPhoto);
        repository.Save();
        return NoContent();
    }
}


