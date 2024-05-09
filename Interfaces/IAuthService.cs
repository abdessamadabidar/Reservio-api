using Microsoft.AspNetCore.Mvc;
using Reservio.Helper;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IAuthService
    {
        public IResult Login(AuthenticateRequest authenticateRequest);
        public IResult Register(RegisterRequest registerUser);
        public bool Verify(Guid Id);
        public bool UserVerified(Guid Id);
        public string GenerateJwtToken(User user);
        public AuthServiceResult ForgotPassword(string email);
        public Guid? ValidateToken(string token);
        
    }
}
