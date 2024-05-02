using Microsoft.AspNetCore.Mvc;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IAuthService
    {
        public IResult Login(LoginUser loginUser);
        public User GetUserByEmail(string email);
    }
}
