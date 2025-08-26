using System.Security.Cryptography;

namespace Web.Api.Services;

public sealed class PasswordHasher
{
    private const int SaltSize = 16; // 128 bit
    private const int HashSize = 32; // 256 bit
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string hashWithSalt)
    {
        var parts = hashWithSalt.Split('-', 2);
        if (parts.Length != 2)
            return false;

        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
        return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
    }

}
