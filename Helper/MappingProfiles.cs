using AutoMapper;
using Reservio.Dto;
using Reservio.Models;

namespace Reservio.Helper
{
    public class MappingProfiles : Profile
    {
       public MappingProfiles()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<UserResponseDto, User>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();


            CreateMap<User, RegisterRequest>();
            CreateMap<RegisterRequest, User>();


            CreateMap<User, RegisterResponse>();
            CreateMap<RegisterResponse, User>();


            CreateMap<Room, RoomResponseDto>();
            CreateMap<RoomResponseDto, Room>();
            
            CreateMap<Room, RoomDto>();
            CreateMap<RoomDto, Room>();


            CreateMap<Notification, NotificationRequestDto>();
            CreateMap<NotificationRequestDto, Notification>();

            CreateMap<Notification, NotificationResponseDto>();
            CreateMap<NotificationResponseDto, Notification>();

            CreateMap<Reservation, ReservationRequestDto>();
            CreateMap<ReservationRequestDto, Reservation>();

            CreateMap<Reservation, ReservationResponseDto>();
            CreateMap<ReservationResponseDto, Reservation>();
          


        }
    }
}
