using Microsoft.IdentityModel.Tokens;
using Reservio.Email;
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
        private readonly IEmailService _emailService;

        public AuthService(IConfiguration configuration, IUserService userService, IEmailService emailService)
        {
            _configuration = configuration;
            _userService = userService;
            _emailService = emailService;
            
        }

        public IResult Login(AuthenticateRequest authenticateRequest)
        {
            if (authenticateRequest == null)
            {
                return Results.BadRequest("Invalid request");
            }

            var user = _userService.GetUserByEmail(authenticateRequest.Email);
            if (user == null)
                return Results.NotFound("User not found!");



            var matches = user != null && BCrypt.Net.BCrypt.Verify(authenticateRequest.Password, user.Password);

            if (!matches)
            {
                return Results.Json("Invalid password!");
            }

            // valid user credentials but not verified
            // send email to user to verify account
            if (user?.VerifiedAt == null)
            {
                // generate template
                var template = _emailService.PrepareEmailTemplate(user.Id, user.FirstName, user.LastName);
                var EmailToSend = new Mail
                {
                    To = user.Email,
                    Subject = "Account Verification",
                    Body = template
                };

                try
                {
                    _emailService.SendEmail(EmailToSend);
                }
                catch
                {
                    throw new Exception("Failed to send email");
                }
                return Results.Json("User not verified! We've sent an email to verify your account");
            }


            // user authenticated successfully so generate jwt token
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

            // check whether user exists and is not deleted
            if (user != null && user.DeletedAt == null)
            {
                Results.BadRequest(user.Email + " already exists");
            }


            var registeredUser = _userService.RegisterUser(registerRequest);
            if (registeredUser is null)
                return Results.Json($"Something went wrong updating the user {registerRequest.Email}");



            // Prepare body of email

           var template = _emailService.PrepareEmailTemplate(registeredUser.Id, registeredUser.FirstName, registeredUser.LastName);

           
            // send email to user
            var EmailToSend = new Mail
            {
                To = registerRequest.Email,
                Subject = "Account Verification",
                Body = template
            };

            try
            {
                _emailService.SendEmail(EmailToSend);
            }
            catch
            {
                throw new Exception("Failed to send email");
            }

            return Results.Ok("User regisered successfully! We've sent an email, so please verify your account");
        }

        public bool Verify(Guid Id)
        {
            return _userService.VerifyUser(Id);
        }

        public bool UserVerified(Guid Id)
        {
            return _userService.UserVerified(Id);
        }
    }
}
