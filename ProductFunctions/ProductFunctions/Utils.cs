using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Text;


namespace StoreFunctions
{
  public static class Utils
  {
    private static JWTGenerator JWTgenerator = new();
    private static int saltLengthLimit = 32;
    public static JsonSerializerSettings deserializeSettings = new JsonSerializerSettings
    {
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore,
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };


    private static byte[] GetSalt()
    {
      return GetSalt(saltLengthLimit);
    }
    private static byte[] GetSalt(int maximumSaltLength)
    {
      var salt = new byte[maximumSaltLength];
      using (var random = new RNGCryptoServiceProvider())
      {
        random.GetNonZeroBytes(salt);
      }

      return salt;
    }

    public static string GenerateRefreshToken()
    {
      var randomNumber = new byte[32];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
      }
    }

    public static string getJWTToken(User user)
    {
      return JWTgenerator.IssuingJWT(user);
    }

    public static ValidateJWT verifyJWTToken(HttpRequest req)
    {
      return new ValidateJWT(req);
    }

    public static string hashPassword(string password, string salt)
    {
      string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: password!, salt: Encoding.ASCII.GetBytes(salt), prf: KeyDerivationPrf.HMACSHA256, iterationCount: 15, numBytesRequested: 32));
      return hashed;
    }

    public static void secureUserFields(User user)
    {
      user.RefreshToken = GenerateRefreshToken();
      user.HashSalt = Encoding.UTF8.GetString(GetSalt());
      user.HashPass = hashPassword(user.HashPass, user.HashSalt);

    }

  }
}




