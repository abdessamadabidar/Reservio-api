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

            CreateMap<User, LoginResponseDto>();
            CreateMap<LoginResponseDto, User>();


            CreateMap<User, RegisterRequest>();
            CreateMap<RegisterRequest, User>();

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();


            CreateMap<User, RegisterResponse>();
            CreateMap<RegisterResponse, User>();

            CreateMap<User, UpdateUserDto>();
            CreateMap<UpdateUserDto, User>();
        

            CreateMap<Room, RoomResponseDto>();
            CreateMap<RoomResponseDto, Room>();
            
            CreateMap<Room, RoomDto>();
            CreateMap<RoomDto, Room>();

            

            CreateMap<Room, RoomRequestDto>();
            CreateMap<RoomRequestDto, Room>();



            CreateMap<Notification, NotificationRequestDto>();
            CreateMap<NotificationRequestDto, Notification>();

            CreateMap<Notification, NotificationResponseDto>();
            CreateMap<NotificationResponseDto, Notification>();

            CreateMap<Reservation, ReservationRequestDto>();
            CreateMap<ReservationRequestDto, Reservation>();

            CreateMap<Reservation, ReservationResponseDto>();
            CreateMap<ReservationResponseDto, Reservation>();
          

            CreateMap<Equipment, EquipmentDto>();
            CreateMap<EquipmentDto, Equipment>();

            CreateMap<RoomEquipment, RoomEquipmentDto>();
            CreateMap<RoomEquipmentDto, RoomEquipment>();


            CreateMap<Reservation, RoomAvailability>();
            CreateMap<RoomAvailability, Reservation>();
        }
    }
}
