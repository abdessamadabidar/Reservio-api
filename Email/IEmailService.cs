using System.Diagnostics.Eventing.Reader;

namespace Reservio.Email
{
    public interface IEmailService
    {
        void SendEmail(Mail mail);
        string PrepareEmailTemplate(string FirstName, string LastName, string url);

    }
}
