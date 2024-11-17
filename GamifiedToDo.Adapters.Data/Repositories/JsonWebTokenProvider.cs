using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GamifiedToDo.Adapters.Data.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GamifiedToDo.Adapters.Data.Repositories;

public interface IJsonWebTokenProvider
{
    JwtSecurityToken GenerateToken(User user);
}

public class JsonWebTokenProvider : IJsonWebTokenProvider
{
    private readonly JsonWebTokenSettings _jwtSettings;

    public JsonWebTokenProvider(IOptions<JsonWebTokenSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    
    public JwtSecurityToken GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Name, user.Id)
        };
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }
}