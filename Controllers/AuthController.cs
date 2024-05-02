using Microsoft.AspNetCore.Mvc;
using Reservio.Email;
using Reservio.Interfaces;
using Reservio.Models;




namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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


    }
}
