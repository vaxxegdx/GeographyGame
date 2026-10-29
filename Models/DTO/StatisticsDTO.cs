namespace GeographyGame.Backend.Models.DTO;

// Рейтинг игроков
public class PlayerRatingDto
{
    public int UserId { get; set; }
    public string Login { get; set; } = "";
    public int MatchesPlayed { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
    public double WinRate { get; set; }
}

// Общая статистика игры
public class GameOverviewDto
{
    public int TotalUsers { get; set; }
    public int TotalCountries { get; set; }
    public int TotalPhotos { get; set; }
    public int TotalMatches { get; set; }
    public int CompletedMatches { get; set; }
    public int WaitingMatches { get; set; }
    public int InProgressMatches { get; set; }
}

// Статистика страны и её фотографий
public class CountryStatisticsDto
{
    public int CountryId { get; set; }
    public string CountryName { get; set; } = "";
    public int UsersCount { get; set; }
    public int PhotosCount { get; set; }
    public int AuthorsCount { get; set; }
    public double Percentage { get; set; }
}

// Статистика авторов фотографий
public class PhotoAuthorStatisticsDto
{
    public int UserId { get; set; }
    public string Login { get; set; } = "";
    public int PhotosCount { get; set; }
    public int CountriesCount { get; set; }
}

// Фотография вместе с названием страны и автором
public class TravelPhotoViewDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Details { get; set; }
    public DateTime? CreationDate { get; set; }
    public int CountryId { get; set; }
    public string CountryName { get; set; } = "";
    public int UserId { get; set; }
    public string AuthorLogin { get; set; } = "";
}
