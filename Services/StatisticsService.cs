
using GeographyGame.Backend.Models;
using GeographyGame.Backend.Models.DTO;
using GeographyGame.Backend.Repository;

namespace GeographyGame.Backend.Services;

public class StatisticsService
{
    private readonly IGeographyGameRepository _repository;

    public StatisticsService(IGeographyGameRepository repository)
    {
        _repository = repository;
    }

    // GET /Statistics/overview
    public GameOverviewDto GetOverview()
    {
        var users = _repository.GetUsersList().ToList();
        var countries = _repository.GetCountriesList().ToList();
        var photos = _repository.GetTravelPhotosList().ToList();
        var matches = _repository.GetMatchesList().ToList();

        return new GameOverviewDto
        {
            TotalUsers = users.Count,
            TotalCountries = countries.Count,
            TotalPhotos = photos.Count,
            TotalMatches = matches.Count,

            CompletedMatches = matches.Count(
                m => m.Status == MatchStatus.Completed),

            WaitingMatches = matches.Count(
                m => m.Status == MatchStatus.Waiting),

            InProgressMatches = matches.Count(
                m => m.Status == MatchStatus.InProgress)
        };
    }

    // GET /Statistics/leaderboard
    public IEnumerable<PlayerRatingDto> GetLeaderboard(int limit)
    {
        var users = _repository.GetUsersList().ToList();

        var matches = _repository.GetMatchesList()
            .Where(m => m.Status == MatchStatus.Completed)
            .ToList();

        return users
            .Where(u => !u.IsAdmin)
            .Select(user =>
            {
                var playerMatches = matches
                    .Where(m =>
                        m.Player1Id == user.Id ||
                        m.Player2Id == user.Id)
                    .ToList();

                int wins = playerMatches.Count(
                    m => m.WinnerId == user.Id);

                int draws = playerMatches.Count(
                    m => m.WinnerId == null);

                int losses = playerMatches.Count(
                    m => m.WinnerId != null &&
                        m.WinnerId != user.Id);

                int played = playerMatches.Count;

                return new PlayerRatingDto
                {
                    UserId = user.Id,
                    Login = user.Login,
                    MatchesPlayed = played,
                    Wins = wins,
                    Losses = losses,
                    Draws = draws,

                    WinRate = played == 0
                        ? 0
                        : Math.Round(100.0 * wins / played, 2)
                };
            })
            .OrderByDescending(p => p.Wins)
            .ThenByDescending(p => p.WinRate)
            .ThenByDescending(p => p.MatchesPlayed)
            .Take(limit)
            .ToList();
    }

    // GET /Statistics/users/{id}
    public PlayerRatingDto? GetPlayerStatistics(int id)
    {
        var user = _repository.GetUser(id);

        if (user == null)
            return null;

        var matches = _repository.GetMatchesList()
            .Where(m =>
                m.Status == MatchStatus.Completed &&
                (m.Player1Id == id || m.Player2Id == id))
            .ToList();

        int wins = matches.Count(m => m.WinnerId == id);
        int draws = matches.Count(m => m.WinnerId == null);
        int losses = matches.Count(
            m => m.WinnerId != null && m.WinnerId != id);

        return new PlayerRatingDto
        {
            UserId = user.Id,
            Login = user.Login,
            MatchesPlayed = matches.Count,
            Wins = wins,
            Losses = losses,
            Draws = draws,

            WinRate = matches.Count == 0
                ? 0
                : Math.Round(
                    100.0 * wins / matches.Count, 2)
        };
    }

    // GET /Statistics/countries
    public IEnumerable<CountryStatisticsDto> GetCountryStatistics()
    {
        var countries = _repository.GetCountriesList().ToList();
        var users = _repository.GetUsersList().ToList();
        var photos = _repository.GetTravelPhotosList().ToList();

        int totalPhotos = photos.Count;

        return countries
            .Select(country =>
            {
                var countryPhotos = photos
                    .Where(p => p.CountryId == country.Id)
                    .ToList();

                return new CountryStatisticsDto
                {
                    CountryId = country.Id,
                    CountryName = country.Name,

                    UsersCount = users.Count(
                        u => u.CountryId == country.Id),

                    PhotosCount = countryPhotos.Count,

                    AuthorsCount = countryPhotos
                        .Select(p => p.UserId)
                        .Distinct()
                        .Count(),

                    Percentage = totalPhotos == 0
                        ? 0
                        : Math.Round(
                            100.0 * countryPhotos.Count / totalPhotos, 2)
                };
            })
            .OrderByDescending(c => c.PhotosCount)
            .ThenBy(c => c.CountryName)
            .ToList();
    }

    // GET /Statistics/photos-by-country/{countryId}
    public IEnumerable<TravelPhotoViewDto>? GetPhotosByCountry(
        int countryId)
    {
        var country = _repository.GetCountry(countryId);

        if (country == null)
            return null;

        var users = _repository.GetUsersList().ToList();

        return _repository.GetTravelPhotosList()
            .Where(p => p.CountryId == countryId)
            .Select(photo => new TravelPhotoViewDto
            {
                Id = photo.Id,
                Title = photo.Title,
                Details = photo.Details,
                CreationDate = photo.CreationDate,
                CountryId = country.Id,
                CountryName = country.Name,
                UserId = photo.UserId,

                AuthorLogin = users
                    .FirstOrDefault(u => u.Id == photo.UserId)?.Login
                    ?? "Unknown"
            })
            .OrderByDescending(p => p.CreationDate)
            .ToList();
    }

    // GET /Statistics/recent-photos
    public IEnumerable<TravelPhotoViewDto> GetRecentPhotos(int limit)
    {
        var countries = _repository.GetCountriesList()
            .ToDictionary(c => c.Id, c => c.Name);

        var users = _repository.GetUsersList()
            .ToDictionary(u => u.Id, u => u.Login);

        return _repository.GetTravelPhotosList()
            .OrderByDescending(p => p.CreationDate.HasValue)
            .ThenByDescending(p => p.CreationDate)
            .ThenByDescending(p => p.Id)
            .Take(limit)
            .Select(photo => new TravelPhotoViewDto
            {
                Id = photo.Id,
                Title = photo.Title,
                Details = photo.Details,
                CreationDate = photo.CreationDate,
                CountryId = photo.CountryId,

                CountryName = countries.GetValueOrDefault(
                    photo.CountryId, "Unknown"),

                UserId = photo.UserId,

                AuthorLogin = users.GetValueOrDefault(
                    photo.UserId, "Unknown")
            })
            .ToList();
    }

    public object GetEffectivenessLeaderboard(int limit)
    {
        var users = _repository.GetUsersList().ToList();

        var matches = _repository.GetMatchesList()
            .Where(m => m.Status == MatchStatus.Completed
                && m.StartedAt.HasValue
                && m.FinishedAt.HasValue)
            .ToList();

        var leaderboardByEff = users.Select(u =>
        {
            var userMatches = matches
                .Where(m => m.Player1Id == u.Id || m.Player2Id == u.Id)
                .ToList();

            return new
            {
                u.Id,
                u.Login,

                MatchesPlayed = userMatches.Count,

                Effectiveness = Math.Round(
                    userMatches.Count > 0
                        ? userMatches.Average(m =>
                            (m.Player1Id == u.Id
                                ? m.Player1Score
                                : m.Player2Score) /
                            (m.FinishedAt!.Value - m.StartedAt!.Value).TotalMinutes)
                        : 0,
                    2)
            };
        })
        .OrderByDescending(u => u.Effectiveness)
        .Take(limit)
        .ToList();

        return leaderboardByEff;
    }
}






