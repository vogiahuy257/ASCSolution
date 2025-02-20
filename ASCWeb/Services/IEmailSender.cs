using Microsoft.AspNetCore.Builder;

namespace ASCWeb.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
