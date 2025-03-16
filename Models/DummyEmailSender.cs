using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace OnlineBookStore.Models
{
    namespace OnlineBookStore.Models
    {
        public class DummyEmailSender : IEmailSender
        {
            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                return Task.CompletedTask;
            }
        }

    }
}
