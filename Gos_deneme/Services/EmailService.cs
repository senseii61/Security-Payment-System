using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Gos_deneme.Services
{
    public class EmailService
    {
        private readonly string _fromAddress = "odemesistemi61@gmail.com";
        private readonly string _appPassword = "mvlpxghifmogriws";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_fromAddress);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(_fromAddress, _appPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }
        // Şifremi unuttum kısmı için
        public async Task SifreSifirlaGonder(string toEmail, string yeniSifre)
        {
            string subject = "Şifre Sıfırlama";
            string body = $"Yeni şifreniz: {yeniSifre}";

            await SendEmailAsync(toEmail, subject, body);
        }
    }

}
