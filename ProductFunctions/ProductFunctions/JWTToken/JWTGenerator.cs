using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using System;
using System.Security.Claims;
using System.Collections.Generic;

namespace StoreFunctions
{
  public class JWTGenerator
  {
    private readonly IJwtAlgorithm _algorithm;
    private readonly IJsonSerializer _serializer;
    private readonly IBase64UrlEncoder _base64Encoder;
    private readonly IJwtEncoder _jwtEncoder;

    [Obsolete]
    public JWTGenerator()
    {
      _algorithm = new HMACSHA256Algorithm();
      _serializer = new JsonNetSerializer();
      _base64Encoder = new JwtBase64UrlEncoder();
      _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);
    }
    public string IssuingJWT(User user)
    {
      Dictionary<string, object> claims = new()
      {
        { "username", user.Email },
        { "name", user.FullName },
        { "address", user.Address },
        { "phone", user.Phone },
        { "role", user.Role },
        { "refreshToken", user.RefreshToken },
        { "expiry", DateTime.UtcNow.AddSeconds(10).ToString() }
      };


      string token = _jwtEncoder.Encode(claims, Environment.GetEnvironmentVariable($"ConnectionStrings:jwtKey", EnvironmentVariableTarget.Process));
      return token;
    }
  }
}
