using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Elfie.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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


        public RoomService(IRoomRepository roomRepository, IMapper mapper, IEquipmentService equipmentService, IWebHostEnvironment environment)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _equipmentService = equipmentService;
            _environment = environment;
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


            _roomRepository.Save();

            
            await UploadImage(roomDto.ImageFile, room.Id);

            string imagePath = GetImageByRoom(room.Id);
            room.ImagePath = imagePath;

           if(!_roomRepository.UpdateRoom(room))
            {
                return Result.UploadImageFailure;
            }


            return Result.Success;

            
        }

        public bool DeleteRoom(Guid roomId)
        {
            var roomToDelete = _roomRepository.GetRoomById(roomId);
            if (roomToDelete == null)
            {
                return false;
            }
            return _roomRepository.DeleteRoom(roomToDelete);
        }

        public ICollection<RoomResponseDto> GetAllRooms()
        {

            return _mapper.Map<List<RoomResponseDto>>(_roomRepository.GetAllRooms()); 
        }

        public async Task<ICollection<RoomAvailability>> GetRoomAvailabilities(Guid roomId, DateTime date)
        {

            return await _roomRepository.roomAvailabilities(roomId, date);
        }

        public Room GetRoomById(Guid roomId)
        {
            return _roomRepository.GetRoomById(roomId);
        }
        public bool RoomExists(Guid id)
        {
            return _roomRepository.RoomExists(id);
        }

        public bool UpadateRoom(RoomResponseDto roomDto)
        {
            var roomMap = _mapper.Map<Room>(roomDto);
            return _roomRepository.UpdateRoom(roomMap);
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
                        await image.CopyToAsync(stream);

                    }


                }
                

            }
            catch
            {
                
            }



        }
    }
}
