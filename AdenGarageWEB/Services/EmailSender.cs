using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;  // SecureSocketOptions için gerekli namespace
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailSender : IEmailSender
{
    private readonly string _smtpServer = "smtp.gmail.com";  // Gmail için SMTP sunucusu
    private readonly int _smtpPort = 587;  // TLS portu
    private readonly string _smtpUsername = "eyupltdexmpl@gmail.com";
    private readonly string _smtpPassword = "ukyg byfh cczi hhvr";  // Gerçek şifrenizi gizli tutun!

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();

        // Gönderenin adı ve e-posta adresi
        message.From.Add(new MailboxAddress("AdenGarage", _smtpUsername));  // "AdenGarage" olarak ad ekleniyor

        // Alıcının e-posta adresi
        message.To.Add(new MailboxAddress(string.Empty, email));

        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            // Bağlantıyı güvenli bir şekilde kurun
            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);  // 'SecureSocketOptions.StartTls' kullanarak STARTTLS kurulur

            // Kimlik doğrulama
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);

            // E-postayı gönder
            await client.SendAsync(message);

            // Bağlantıyı kapat
            await client.DisconnectAsync(true);
        }
    }
}
