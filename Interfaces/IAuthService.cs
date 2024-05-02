using Microsoft.AspNetCore.Mvc;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IAuthService
    {
        public IResult Login(AuthenticateRequest authenticateRequest);
        public IResult Register(RegisterRequest registerUser);
    }
}
