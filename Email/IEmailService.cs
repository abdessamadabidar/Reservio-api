using System.Diagnostics.Eventing.Reader;

namespace Reservio.Email
{
    public interface IEmailService
    {
        void SendEmail(Mail mail);
        string PrepareEmailTemplate(Guid Id, string FirstName, string LastName);

    }
}
