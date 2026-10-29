using GeographyGame.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeographyGame.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly StatisticsService _statisticsService;

    public StatisticsController(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("overview")]
    public IActionResult GetOverview()
    {
        return Ok(_statisticsService.GetOverview());
    }

    [HttpGet("leaderboard")]
    public IActionResult GetLeaderboard([FromQuery] int limit = 10)
    {
        if (limit < 1 || limit > 100)
            return BadRequest("Limit must be between 1 and 100.");

        return Ok(_statisticsService.GetLeaderboard(limit));
    }

    [HttpGet("effectiveness-leaderboard")]
    public IActionResult GetEffectivenessLeaderboard([FromQuery] int limit = 10)
    {
        if (limit < 1 || limit > 100)
            return BadRequest("Limit must be between 1 and 100.");

        return Ok(_statisticsService.GetEffectivenessLeaderboard(limit));
    }

    [HttpGet("users/{id}")]
    public IActionResult GetPlayerStatistics(int id)
    {
        if (id < 1)
            return BadRequest("Invalid user ID.");

        var result = _statisticsService.GetPlayerStatistics(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("countries")]
    [HttpGet("photo-countries")]
    public IActionResult GetCountryStatistics()
    {
        return Ok(_statisticsService.GetCountryStatistics());
    }

    [HttpGet("photos-by-country/{countryId}")]
    public IActionResult GetPhotosByCountry(int countryId)
    {
        if (countryId < 1)
            return BadRequest("Invalid country ID.");

        var result = _statisticsService.GetPhotosByCountry(countryId);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("recent-photos")]
    public IActionResult GetRecentPhotos([FromQuery] int limit = 10)
    {
        if (limit < 1 || limit > 100)
            return BadRequest("Limit must be between 1 and 100.");

        return Ok(_statisticsService.GetRecentPhotos(limit));
    }
}
