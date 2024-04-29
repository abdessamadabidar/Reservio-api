﻿using System.ComponentModel.DataAnnotations;

namespace Reservio.Dto {
    public class RoomDto {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public bool isReserved { get; set; }
    }
}
