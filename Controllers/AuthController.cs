using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Reservio.Dto;
using Reservio.Email;
using Reservio.Interfaces;
using Reservio.Models;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Text;
using Reservio.Services;
using Reservio.Helper;
using System.Runtime.CompilerServices;
using AutoMapper;




namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper; 

        public AuthController(IAuthService authService, IUserService userService, IEmailService emailService, IMapper mapper)
        {
            _authService = authService;
            _userService = userService;
            _emailService = emailService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IResult Login([FromBody] AuthenticateRequest authenticateRequest)
        {

            if(!ModelState.IsValid)
            {
                return Results.BadRequest(ModelState);
            }

            return _authService.Login(authenticateRequest);

        }


        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IResult Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return Results.BadRequest(ModelState);
            }

            return _authService.Register(registerRequest);
        }

        [HttpGet("verify/{Id:guid}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult Verify(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return BadRequest("Invalid request");
            }

            if (_authService.UserVerified(Id))
            {
                return BadRequest("User is already verified");
            }

            if (!_authService.Verify(Id))
            {
                return BadRequest("Could not verify the user");
            }


            return Ok("Account verified successfully");
        }



        [HttpPost("forgot-password")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordRequest)
        {
            if (!ModelState.IsValid || forgotPasswordRequest == null)
            {
                return BadRequest("Invalid request");
            }


            var result = _authService.ForgotPassword(forgotPasswordRequest.Email);

            if (result == AuthServiceResult.UserNotFound)
            {
                return NotFound("User not found");
            }

           
            if (result == AuthServiceResult.EmailSendFailure)
            {
                return BadRequest("Could not send email");
            }

  

            return Ok("Email sent successfully");
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            if (!ModelState.IsValid || resetPasswordRequestDto == null)
            {
                return BadRequest("Invalid request");
            }

            
            var userId = _authService.ValidateToken(resetPasswordRequestDto.Token);
            if (userId == null || (userId.HasValue && !_userService.UserExists(userId.Value)))
                return BadRequest("Invalid or expired password token");
             

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordRequestDto.Password);
            User user = _userService.GetUserById(userId.Value);
            user.Password = hashedPassword;
            if (!_userService.UpdateUser(user))
            {
                return BadRequest("Could not reset password");
            }

           

            return Ok("Password reset successfully");
        }
    }
}
