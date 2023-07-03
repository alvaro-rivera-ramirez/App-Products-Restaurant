using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ApiRestaurant.DTO;

namespace ApiRestaurant.Services
{
    public class HandleToken
    {
        private readonly IConfiguration _configuration;
        public HandleToken(IConfiguration configuration) { 
            _configuration= configuration;
        }

        public string generateToken(UserListDTO payload)
        {

            var claims = new ClaimsIdentity ();

            claims.AddClaim(new Claim("id", payload.id.ToString()));
            claims.AddClaim(new Claim("fullname", payload.fullname));
            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:jwt_key").Value));
            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig=tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(tokenConfig);

            return token;
        }
    }
}
