namespace PoorMansDeck.Server.Security;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

// TODO setting base
internal static class TokenHelper
{
    public static string Generate()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = new JwtSecurityToken(
            AuthenticationSettings.Issuer,
            AuthenticationSettings.Audience,
            new List<Claim>(),
            expires: DateTime.UtcNow.AddDays(AuthenticationSettings.ExpireDays),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationSettings.SecretKey)), SecurityAlgorithms.HmacSha256Signature));
        return tokenHandler.WriteToken(token);
    }
}
