using System.Data.Common;
using GeographyGame.Backend.Data;
using GeographyGame.Backend.Models;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace GeographyGame.Backend.Repository;

public enum DeleteCountryResult
{
    Deleted,
    NotFound,
    HasPhotos
}

public class GeographyGameRepository : IGeographyGameRepository
{
    private readonly GeographyGameContext _db;
    private bool _disposed = false;

    public GeographyGameRepository(GeographyGameContext context)
    {
        _db = context;
    }

    public virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Save()
    {
        _db.SaveChanges();
    }

    public IEnumerable<Country> GetCountriesList()
    {
        return _db.Countries;
    }

    public Country GetCountry(int id)
    {
        return _db.Countries.Find(id);
    }
    public void CreateCountry(Country country)
    {
        _db.Countries.Add(country);
    }
    public void UpdateCountry(Country country)
    {
        _db.Entry(country).State = EntityState.Modified;

    }
    public DeleteCountryResult DeleteCountry(int id)
    {
        var country = _db.Countries.Find(id);

        if (country == null)
            return DeleteCountryResult.NotFound;

        bool hasPhotos = _db.TravelPhotos
            .Any(photo => photo.CountryId == id);

        if (hasPhotos)
            return DeleteCountryResult.HasPhotos;

        _db.Countries.Remove(country);
        return DeleteCountryResult.Deleted;
    }

    public IEnumerable<TravelPhoto> GetTravelPhotosList()
    {
        return _db.TravelPhotos;
    }
    public TravelPhoto GetTravelPhoto(int id)
    {
        return _db.TravelPhotos.Find(id);
    }
    public void CreateTravelPhoto(TravelPhoto photo)
    {
        _db.TravelPhotos.Add(photo);
    }
    public void UpdateTravelPhoto(TravelPhoto photo)
    {
        _db.Entry(photo).State = EntityState.Modified;
    }
    public bool DeleteTravelPhoto(int id)
    {
        var photo = _db.TravelPhotos.Find(id);
        if (photo != null)
        {
            _db.TravelPhotos.Remove(photo);
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerable<User> GetUsersList()
    {
        return _db.Users;
    }
    public User GetUser(int id)
    {
        return _db.Users.Find(id);
    }
    public void CreateUser(User user)
    {
        _db.Users.Add(user);
    }
    public void UpdateUser(User user)
    {
        _db.Entry(user).State = EntityState.Modified;
    }
    public bool DeleteUser(int id)
    {
        var user = _db.Users.Find(id);
        if (user != null)
        {
            _db.Users.Remove(user);
            return true;
        } else
        {
            return false;
        }
    }

    public IEnumerable<Match> GetMatchesList()
    {
        return _db.Matches;
    }
    public Match GetMatch(int id)
    {
        return _db.Matches.Find(id);
    }
    public void CreateMatch(Match match)
    {
        _db.Matches.Add(match);
    }
    public void UpdateMatch(Match match)
    {
        _db.Entry(match).State = EntityState.Modified;
    }
    public bool DeleteMatch(int id)
    {
        var match = _db.Matches.Find(id);

        if (match != null)
        {
            _db.Matches.Remove(match);
            return true;
        }
        else
        {
            return false;
        }
    }

}