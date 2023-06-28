using System;
using Mentore.Models.DTOs.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using System.Threading.Tasks;
using CloudinaryDotNet;

namespace Mentore.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        public EmailSender(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailVerificationAsync(string toEmail, string code, string emailFor)
        {
            try
            {
                var api = "http://localhost:41783/api/user/" + emailFor + "?code=" + code;
                string subject = "";
                string body = "";

                if (emailFor == "verify-account")
                {
                    subject = "Mentore - Xác thực Email để kích hoạt tài khoản!";
                    body = "<h3>BƯỚC CUỐI CÙNG ĐẺ KÍCH HOẠT TÀI KHOẢN.</h3> " +
                       "<br/>Vui lòng click vào link để xác nhận Email của tài khoản" +
                       "<a href =" + api + "> Verify Account link</a>";
                }
                else if (emailFor == "reset-password")
                {
                    subject = "Mentore - Thay đổi mật khẩu";
                    body = "XIN CHÀO! , <br/><br/>Chúng tôi nhận được yêu cầu thay đổi mật khẩu của bạn. Vui lòng click vào link bên dưới để thay đổi" +
                        "<br/><br/><a href =" + api + ">Reset Password link</a>";
                }

                var builder = new BodyBuilder
                {
                    HtmlBody = body
                };

                var email = new MimeMessage
                {
                    Body = builder.ToMessageBody(),
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await client.SendAsync(email);
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendEmailPaySuccessAsync(string toEmail, string code, string workshopName)
        {
            try
            {
                string subject = $"MENTORE - THANH TOÁN THÀNH CÔNG!";
                string body = $"<h3>Thanh toán vé tham dự Workshop: {workshopName}</h3> " +
                       $"<br/>Mã vé: <p style='font-size=bold'>{code}<p/> <br/> Vui lòng đừng chia sẻ mã cho bất kỳ ai! <br/>Trân trọng, <br/>Mentore";
              
                var builder = new BodyBuilder
                {
                    HtmlBody = body
                };

                var email = new MimeMessage
                {
                    Body = builder.ToMessageBody(),
                    Sender = MailboxAddress.Parse(_mailSettings.Mail)
                };

                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await client.SendAsync(email);
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}