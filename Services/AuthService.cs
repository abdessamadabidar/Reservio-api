using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Reservio.Interfaces;
using Reservio.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Reservio.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
            
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public IResult Login(LoginUser loginUser)
        {
            var user = _userService.GetUserByEmail(loginUser.Email);
            if (user is null)
                return Results.NotFound("User not found!");



            var matches = user != null && BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password);

            if (!matches)
            {
                return Results.Json("Invalid password!");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            };
            foreach (var role in _userService.GetUserRoles(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var token = new JwtSecurityToken
        (
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(60),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                SecurityAlgorithms.HmacSha256)
        );



            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(tokenString);
        }

       
    }
}
