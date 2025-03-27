
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ASCWeb.Configuration;
using ASCWeb.Solution.Services;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ASCWeb.Web.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public AuthMessageSender(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Admin", _settings.Value.SMTPAccount));
            emailMessage.To.Add(new MailboxAddress(email, email)); // 👈 Không cần đặt "User"
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message }; // 👈 Dùng HTML thay vì plain text

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(
                    _settings.Value.SMTPServer,
                    _settings.Value.SMTPPort,
                    SecureSocketOptions.StartTls // 👈 Đảm bảo dùng STARTTLS nếu SMTP yêu cầu
                );

                await client.AuthenticateAsync(_settings.Value.SMTPAccount, _settings.Value.SMTPPassword);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi gửi email: {ex.Message}");
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
        public Task SendSmsAsync(string number, string message)
        {
            throw new NotImplementedException();
        }
    }
}
