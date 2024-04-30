using AutoMapper;
using Reservio.Dto;
using Reservio.Models;

namespace Reservio.Helper
{
    public class MappingProfiles : Profile
    {
       public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
