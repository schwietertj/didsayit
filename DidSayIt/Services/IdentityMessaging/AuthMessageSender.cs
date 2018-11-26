using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DidSayIt.Services.IdentityMessaging
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SendSmsAsync(string number, string message)
        {
            throw new System.NotImplementedException();
        }
    }

}
