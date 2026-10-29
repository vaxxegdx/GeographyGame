using System.Text;
using System.Security.Cryptography;
using GeographyGame.Backend.Utils;

namespace GeographyGame.Backend.Models;

public class User
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int? Matches { get; set; }
    public int? Wins { get; set; }

    public bool CheckPassword(string password) => Hasher.GetHash(password) == PasswordHash;

    public bool IsAdmin => Login == "admin";
}