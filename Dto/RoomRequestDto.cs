namespace Reservio.Dto
{
    public class RoomRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public IFormFile? ImageFile { get; set; } = null!;
    }
}
