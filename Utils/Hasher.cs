using System.Text;
using System.Security.Cryptography;

namespace GeographyGame.Backend.Utils;

public class Hasher
{
    public static string GetHash(string password)
    {
        var hash = Convert.ToHexString(
            MD5.HashData(Encoding.UTF8.GetBytes(password))
        ).ToLowerInvariant();

        return hash;
    }
}