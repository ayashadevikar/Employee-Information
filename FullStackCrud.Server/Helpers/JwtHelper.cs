using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace FullStackCrud.Server.Helpers
{
    public class JwtHelper
    {
        public static string GenerateJwtToken(string username, string key, string issuer, string audience)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username)
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        //var claims = new[]
        //{
        //    new Claim(ClaimTypes.Name, username)
        //};

        //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //var token = new JwtSecurityToken(
        //    issuer: "FullStackCrud.Server",
        //    audience: "fullstackcrud.client",
        //    claims: claims,
        //    expires: DateTime.Now.AddHours(2),
        //    signingCredentials: credentials
        //);

        //return new JwtSecurityTokenHandler().WriteToken(token);
    }
    }

