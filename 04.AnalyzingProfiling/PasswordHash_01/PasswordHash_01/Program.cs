using System.Diagnostics;
using System.Security.Cryptography;

var password = "Password123";
var salt = CreateSalt(16);
var stopWatch = new Stopwatch();
stopWatch.Start();
var hash = GeneratePasswordHashUsingSalt(password, salt);
stopWatch.Stop();

Console.WriteLine($"Hash - {hash}");
Console.WriteLine($"Salt - {Convert.ToBase64String(salt)}");
Console.WriteLine($"Time - {stopWatch.Elapsed}");
Console.ReadLine();

string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
{
    var iterate = 10000000;
    var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
    byte[] hash = pbkdf2.GetBytes(20);

    byte[] hashBytes = new byte[36];
    Array.Copy(salt, 0, hashBytes, 0, 16);
    Array.Copy(hash, 0, hashBytes, 16, 20);
    var passwordHash = Convert.ToBase64String(hashBytes);

    return passwordHash;
}

byte[] CreateSalt(int size)
{
    return RandomNumberGenerator.GetBytes(size);
}
