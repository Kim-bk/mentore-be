using System;
using Mentore.Models.DTOs.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using System.Threading.Tasks;
using API.Model.DTOs;
using System.Collections.Generic;

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

        public async Task SendEmailAppointment(AppointmentEmailDTO model, string action)
        {
            try
            {
                string api, subject, body;
                List<string> emailsToSend = new();
                if (action == "cancelAppointment")
                {
                    subject = $"MENTORE - HỦY LỊCH HẸN!";
                    body = $"<h3>*Cuộc hẹn vào lúc {model.DateTime} đã bị hủy!" +
                           $"<br/>* Tiêu đề: {model.Title}. <br/> Chi tiết: {model.Details.Replace("\n", "<br/>")}" +
                           $"<br/>Trân trọng, <br/>Mentore";

                    emailsToSend.Add(model.MenteeEmail);
                }    
                else if (action == "createAppointment")
                {
                    api = "http://localhost:41783/api/appointment/verify" + "?code=" + model.VerifiedCode;

                    subject = $"MENTORE - CÓ LỊCH HẸN MỚI!";
                    body = $"<h3>*Bạn có lịch hẹn vào lúc {model.DateTime} với {model.MenteeName}</h3> " +
                           $"<br/>*Tiêu đề: {model.Title}. <br/>*Chi tiết: {model.Details.Replace("\n", "<br/>")}" +
                           $"<br/>*Nhấn vào đây để xác nhận cuộc họp: <a href =" + api + ">Link</a>" +
                           $"<br/>*Link cuộc họp: {model.LinkGoogleMeet} <br/>Trân trọng, <br/>Mentore";
                }
                else
                {
                    subject = $"MENTORE - LỊCH HẸN ĐƯỢC CẬP NHẬT!";
                    body = $"<h3>*Bạn có lịch hẹn được cập nhật diễn ra vào lúc {model.DateTime} với {model.MenteeName}</h3> " +
                           $"<br/>*Tiêu đề: {model.Title}. <br/>*Chi tiết: {model.Details.Replace("\n", "<br/>")}" +
                           $"<br/>*Link cuộc họp: {model.LinkGoogleMeet} <br/>Trân trọng, <br/>Mentore";
                }

                emailsToSend.Add(model.MentorEmail);

                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                foreach (var mail in emailsToSend)
                {
                    var builder = new BodyBuilder
                    {
                        HtmlBody = body
                    };

                    var email = new MimeMessage
                    {
                        Body = builder.ToMessageBody(),
                        Sender = MailboxAddress.Parse(_mailSettings.Mail)
                    };

                    email.To.Add(MailboxAddress.Parse(mail));
                    email.Subject = subject;
                    await client.SendAsync(email);
                }    
            
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}