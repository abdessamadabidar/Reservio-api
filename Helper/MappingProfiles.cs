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
            CreateMap<Room, RoomDto>();
            CreateMap<RoomDto, Room>();
            CreateMap<User, RegisterRequest>();
            CreateMap<RegisterRequest, User>();
            CreateMap<User, RegisterResponse>();
            CreateMap<RegisterResponse, User>();
        }
    }
}
