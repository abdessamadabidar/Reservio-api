namespace Reservio.Email
{
    public class EmailConfiguration
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Host { get; set; } = null!;
        public string Displayname { get; set; } = null!;
        public int Port { get; set; }
    }
}
