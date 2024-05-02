using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IResult Login([FromBody] LoginUser loginUser)
        {

            if(!ModelState.IsValid)
            {
                return Results.BadRequest(ModelState);
            }

            return _authService.Login(loginUser);

          
        }

    }
}
