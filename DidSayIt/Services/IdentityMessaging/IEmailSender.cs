using System.Threading.Tasks;

namespace DidSayIt.Services.IdentityMessaging
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string subject, string message);
    }
}
