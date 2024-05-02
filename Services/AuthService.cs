using Microsoft.AspNetCore.Authentication;
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

        public IResult Login(AuthenticateRequest authenticateRequest)
        {
            var user = _userService.GetUserByEmail(authenticateRequest.Email);
            if (user is null)
                return Results.NotFound("User not found!");



            var matches = user != null && BCrypt.Net.BCrypt.Verify(authenticateRequest.Password, user.Password);

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

        public IResult Register(RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                 return Results.BadRequest("Invalid request");
            }

            var user = _userService.GetUserByEmail(registerRequest.Email);
            if (user != null)
            {
                Results.BadRequest(user.Email + " already exists");
            }



            if(!_userService.RegisterUser(registerRequest))
                return Results.Json($"Something went wrong updating the user {registerRequest.Email}");


            // send email to user

            return Results.Ok("User regisered successfully!");
        }


    }
}
