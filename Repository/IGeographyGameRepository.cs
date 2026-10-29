using GeographyGame.Backend.Models;

namespace GeographyGame.Backend.Repository;

public interface IGeographyGameRepository : IDisposable
{
    // countries
    IEnumerable<Country> GetCountriesList();
    Country GetCountry(int id);
    void CreateCountry(Country country);
    void UpdateCountry(Country country);
    DeleteCountryResult DeleteCountry(int id);

    // photos
    IEnumerable<TravelPhoto> GetTravelPhotosList();
    TravelPhoto GetTravelPhoto(int id);
    void CreateTravelPhoto(TravelPhoto photo);
    void UpdateTravelPhoto(TravelPhoto photo);
    bool DeleteTravelPhoto(int id);

    // users
    IEnumerable<User> GetUsersList();
    User GetUser(int id);
    void CreateUser(User user);
    void UpdateUser(User user);
    bool DeleteUser(int id);

    // matches
    IEnumerable<Match> GetMatchesList();
    Match GetMatch(int id);
    void CreateMatch(Match match);
    void UpdateMatch(Match match);
    bool DeleteMatch(int id);

    void Save();
}