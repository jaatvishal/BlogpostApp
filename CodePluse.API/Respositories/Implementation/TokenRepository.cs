using CodePluse.API.Respositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodePluse.API.Respositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;  
        public  TokenRepository(IConfiguration configuration)
        {
           this.configuration = configuration;
        } 
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            //Create Claims
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Email,user.Email)
             };

            claims.AddRange(roles.Select(role=> new Claim(ClaimTypes.Role,role)));
            //jwt security token parameters
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token=new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token expiration time
                signingCredentials: credentials
            );

            //Return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
