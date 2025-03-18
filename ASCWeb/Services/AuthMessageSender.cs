
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
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Admin", _settings.Value.SMTPAccount));
                emailMessage.To.Add(new MailboxAddress("User", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("plain") { Text = message };

                using (var client = new SmtpClient())
                {
                    // Kết nối đến SMTP server với chế độ bảo mật Auto (STARTTLS hoặc SSL/TLS)
                    await client.ConnectAsync(_settings.Value.SMTPServer, _settings.Value.SMTPPort, SecureSocketOptions.StartTls);

                    // Xác thực với SMTP server
                    await client.AuthenticateAsync(_settings.Value.SMTPAccount, _settings.Value.SMTPPassword);

                    // Gửi email
                    await client.SendAsync(emailMessage);

                    // Ngắt kết nối
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                throw;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            throw new NotImplementedException();
        }
    }
}
