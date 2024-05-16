using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Reservio.Dto;
using Reservio.Email;
using Reservio.Helper;
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
        private readonly IMapper _mapper;

        public AuthService(IConfiguration configuration, IUserService userService, IEmailService emailService, IMapper mapper)
        {
            _configuration = configuration;
            _userService = userService;
            _emailService = emailService;
            _mapper = mapper;
        }

        public LoginResponseDto Login(AuthenticateRequest authenticateRequest)
        {
           
            var user = _userService.GetUserByEmail(authenticateRequest.Email);

            // valid user credentials but not verified
            // send email to user to verify account
            if (user?.VerifiedAt == null)
            {
                // generate template
                var message = "You are attempting to sign in to your account without being verified, please verify your account by clicking the button below to sign in successfully.";
                var url = $"https://localhost:7154/api/Auth/Verify/{user?.Id}";
                var template = _emailService.PrepareEmailTemplate(user?.FirstName, user?.LastName, message, url);
                var EmailToSend = new Mail
                {
                    To = user.Email,
                    Subject = "Account Verification",
                    Body = template
                };

                try
                {
                    _emailService.SendEmail(EmailToSend);
                    return new LoginResponseDto
                    {
                        statusCode = 401,
                        message = "Account not verified"
                    };

                }
                catch
                {
                    throw new Exception("Failed to send email");
                }
               


            }



            // user authenticated successfully so generate jwt token and return user details
            var tokenString = GenerateJwtToken(user);
            var userDto = _mapper.Map<LoginResponseDto>(user);
            userDto.Token = tokenString;
            userDto.Roles  = _userService.GetUserRoles(user.Id);

            return userDto;
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
                return Results.BadRequest(user.Email + " already exists");
            }


            var registeredUser = _userService.RegisterUser(registerRequest);
            if (registeredUser is null)
                return Results.Json($"Something went wrong updating the user {registerRequest.Email}");



            // Prepare body of email
            var message = "We are pleased to welcome you to <b>Reservio</b>, your meeting room reservation platform! This email confirms the creation of your account on our platform and we look forward to assisting you with your room reservation needs.";
            var url = $"https://localhost:7154/api/Auth/Verify/{user?.Id}";
            var template = _emailService.PrepareEmailTemplate(registeredUser.FirstName, registeredUser.LastName, message, url);

           
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

            return Results.Ok("You have registered successfully! We've sent you an email, so please verify your account");
        }

        public bool Verify(Guid Id)
        {
            return _userService.VerifyUser(Id);
        }

        public bool UserVerified(Guid Id)
        {
            return _userService.UserVerified(Id);
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            };
            foreach (var role in _userService.GetUserRoles(user.Id))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var token = new JwtSecurityToken
        (
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                SecurityAlgorithms.HmacSha256)
        );


           return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public Result ForgotPassword(string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
            {
                return Result.UserNotFound;
            }
            try
            {

                var message = "You are attempting to reset your password, please click the button below to reset your password.";

                // generate token and encode it
                var token = GenerateJwtToken(user);
               //  var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var url = $"http://localhost:3000/reset-password?token={token}";

                // send email to user to reset password
                var template = _emailService.PrepareEmailTemplate(user.FirstName, user.LastName, message, url);
                var EmailToSend = new Mail
                {
                    To = user.Email,
                    Subject = "Password Reset",
                    Body = template
                };

            
                _emailService.SendEmail(EmailToSend);
                return Result.Success;
            }
            catch
            {
                return Result.EmailSendFailure;
            }
        }

        public Guid? ValidateToken(string token)
        {
            if (token == null)
                return null;


            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return new Guid(userIdClaim.Value);
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
