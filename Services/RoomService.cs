using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Elfie.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Reservio.Data;
using Reservio.Dto;
using Reservio.Helper;
using Reservio.Interfaces;
using Reservio.Models;
using Reservio.Repositories;
using System.Runtime.InteropServices.JavaScript;

namespace Reservio.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IEquipmentService _equipmentService;
        private readonly IWebHostEnvironment _environment;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;




        public RoomService(IRoomRepository roomRepository, IMapper mapper, IEquipmentService equipmentService, IWebHostEnvironment environment, INotificationService notificationService, IUserService userService)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _equipmentService = equipmentService;
            _environment = environment;
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task<Result> CreateRoom(RoomDto roomDto)
        {



            var room = new Room
            {
                Name = roomDto.Name,
                Capacity = roomDto.Capacity,
                Description = roomDto.Description,
                ImagePath = null,
            };



            // Save room to database
            if (!await _roomRepository.CreateRoom(room))
            {
                return Result.CreateRoomFailure;
            }



            // Save equipment to database
            foreach (var equipmentDto in roomDto.Equipments)
            {
                var equipment = await _equipmentService.GetEquipmentById(equipmentDto.Id);

                if (equipment != null)
                {
                    room.RoomEquipments.Add(new RoomEquipment
                    {
                        EquipmentId = equipmentDto.Id,
                        RoomId = room.Id,
                        Room = room,

                    });
                }
            }


            await _roomRepository.Save();


            await UploadImage(roomDto?.ImageFile, room.Id);

            string imagePath = GetImageByRoom(room.Id);
            room.ImagePath = imagePath;

            if (!await _roomRepository.UpdateRoom(room))
            {
                return Result.UploadImageFailure;
            }


            // send notification to admin
            var adminsId = await _userService.GetAdmins();
            string title = "New Room";
            string body = $"New room with name {room.Name} has been added successfully to the database";

            _notificationService.Notifiy(adminsId, title, body);

            return Result.Success;


        }

        public async Task<bool> DeleteRoom(Guid roomId)
        {
            var roomToDelete = await _roomRepository.GetRoomById(roomId);
            if (roomToDelete == null)
            {
                return false;
            }
            return await _roomRepository.DeleteRoom(roomToDelete);
        }

        public ICollection<RoomResponseDto> GetAllRooms()
        {

            return _mapper.Map<List<RoomResponseDto>>(_roomRepository.GetAllRooms());
        }

        public async Task<ICollection<RoomAvailability>> GetRoomAvailabilities(Guid roomId, DateTime date)
        {

            return await _roomRepository.roomAvailabilities(roomId, date);
        }

        public async Task<RoomResponseDto> GetRoomResponseById(Guid roomId)
        {
            var room = _mapper.Map<RoomResponseDto>(await _roomRepository.GetRoomById(roomId));
            return room;
        }


        public bool RoomExists(Guid id)
        {
            return _roomRepository.RoomExists(id);
        }



        public async Task<Result> UpdateRoom(RoomRequestDto roomDto)
        {

            // Fetch the room entity asynchronously
            var room = await _roomRepository.GetRoomById(roomDto.Id);

            if (room == null)
            {
                return Result.RoomNotFound;
            }



            // Handle image upload
            try
            {
                await UploadImage(roomDto?.ImageFile, room.Id);
            }
            catch
            {
                return Result.UploadImageFailure;
            }

            // Retrieve the image path
            string imagePath = GetImageByRoom(room.Id);


            // Update room details
            room.Name = roomDto.Name;
            room.Capacity = roomDto.Capacity;
            room.Description = roomDto.Description;
            room.ImagePath = imagePath;


            // Update the room entity in the repository
            bool updateResult = await _roomRepository.UpdateRoom(room);
            if (!updateResult)
            {
                return Result.UpdateRoomFailure;
            }

            return Result.Success;





        }

        public async Task<Result> UpdateUpdateRoomEquipments(Guid roomId, ICollection<Guid> equipmentIds)
        {
            if (equipmentIds == null)
            {
                return Result.UpdateRoomFailure;
            }

            await _roomRepository.ClearRoomEquipments(roomId);


            foreach (var equipmentId in equipmentIds)
            {
                var equipment = await _equipmentService.GetEquipmentById(equipmentId);
                if (equipment != null)
                {
                    await _roomRepository.AddRoomEquipments(roomId, equipmentId);
                }

            }

            return Result.Success;
        }

        private string GetFilePath(Guid roomId)
        {
            return this._environment.WebRootPath + "\\Uploads\\Room\\" + roomId;
        }

        private string GetImageByRoom(Guid roomId)
        {
            string ImageUrl = string.Empty;
            string HostUrl = "https://localhost:7154/";
            string Filepath = GetFilePath(roomId);
            string Imagepath = Filepath + "\\image.jpg";
            if (!System.IO.File.Exists(Imagepath))
            {
                ImageUrl = HostUrl + "/uploads/common/placeholder.svg";
            }
            else
            {
                ImageUrl = HostUrl + "/uploads/Room/" + roomId + "/image.jpg";
            }



            return ImageUrl;

        }

        async Task<Room> IRoomService.GetRoomById(Guid roomId)
        {
            return await _roomRepository.GetRoomById(roomId);
        }

        private async Task UploadImage(IFormFile image, Guid roomId)
        {

            try
            {


                if (image != null && image.Length > 0)
                {

                    var fileName = Path.GetFileName(image?.FileName);
                    var FilePath = GetFilePath(roomId);




                    if (!System.IO.Directory.Exists(FilePath))
                    {
                        System.IO.Directory.CreateDirectory(FilePath);
                    }


                    string ImagePath = FilePath + "\\image.jpg";


                    if (System.IO.File.Exists(ImagePath))
                    {
                        System.IO.File.Delete(ImagePath);
                    }


                    using (FileStream stream = System.IO.File.Create(ImagePath))
                    {
                        await image?.CopyToAsync(stream);

                    }


                }


            }
            catch
            {

            }



        }
    }
}
