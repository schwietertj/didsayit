using System.Threading.Tasks;

namespace DidSayIt.Services.IdentityMessaging
{
    public interface ISmsSender
    {
        Task<bool> SendSmsAsync(string number, string message);
    }

}
