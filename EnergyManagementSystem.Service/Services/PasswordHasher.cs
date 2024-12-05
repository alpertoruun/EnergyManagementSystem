using System.Security.Cryptography;
using EnergyManagementSystem.Core.Interfaces;
using EnergyManagementSystem.Core.Interfaces.IService;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16; // 128 bit
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private const char Delimiter = ';';

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            _hashAlgorithmName,
            KeySize
        );

        return string.Join(
            Delimiter,
            Convert.ToBase64String(salt),
            Convert.ToBase64String(hash),
            Iterations,
            _hashAlgorithmName
        );
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var elements = hashedPassword.Split(Delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);
        var iterations = int.Parse(elements[2]);
        var hashAlgorithmName = new HashAlgorithmName(elements[3]);

        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            hashAlgorithmName,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
    }
}