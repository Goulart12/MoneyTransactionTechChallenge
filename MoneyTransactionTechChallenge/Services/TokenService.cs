using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MoneyTransactionTechChallenge.Models;

namespace MoneyTransactionTechChallenge.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
      
        var key = Encoding.ASCII.GetBytes(Configuration.JWTToken);
      
        // var claims = user.GetClaims();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(), //Claims que vão compor o token
            Expires = DateTime.UtcNow.AddHours(8), //Por quanto tempo vai valer o token?
            SigningCredentials = //Assinatura do token, serve para identificar que mandou o token e garantir que o token não foi alterado no meio do caminho.
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
        };

      
        var token = tokenHandler.CreateToken(tokenDescriptor);
      
        return tokenHandler.WriteToken(token);
    }
}