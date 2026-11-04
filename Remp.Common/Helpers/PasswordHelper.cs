using System.Security.Cryptography;

namespace Remp.Common.Helpers;

public static class PasswordHelper
{
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Numbers = "0123456789";
    private const string Special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

    public static string GeneratePassword(int length = 8)
    {
        if (length < 4)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Password length must be at least 4.");
        }

        string validChars = Lowercase + Uppercase + Numbers + Special;
        var password = new char[length];

        password[0] = Lowercase[RandomNumberGenerator.GetInt32(Lowercase.Length)];
        password[1] = Uppercase[RandomNumberGenerator.GetInt32(Uppercase.Length)];
        password[2] = Numbers[RandomNumberGenerator.GetInt32(Numbers.Length)];
        password[3] = Special[RandomNumberGenerator.GetInt32(Special.Length)];

        for (int i = 4; i < length; i++)
        {
            password[i] = validChars[RandomNumberGenerator.GetInt32(validChars.Length)];
        }

        for (int i = password.Length - 1; i > 0; i--)
        {
            int index = RandomNumberGenerator.GetInt32(i + 1);
            (password[i], password[index]) = (password[index], password[i]);
        }

        return new string(password);
    }
}
