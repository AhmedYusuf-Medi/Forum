using Forum.Models.Response;
using Forum.Service.Contracts;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using static Forum.Service.Common.Message.Message;

namespace Forum.Service
{
    public class MailService : IMailService
    {
        public async Task<InfoResponse> SendMailAsync(string email, Guid code)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(ExternalProvider.From_Title, ExternalProvider.Forum_Mail));
            message.To.Add(new MailboxAddress(ExternalProvider.To_Title, email));
            message.Subject = ExternalProvider.Mail_Subject;
            message.Body = new TextPart("plain")
            { Text = $"http://localhost:37954/account/verification?email={email}&code={code}" };

            using (var client = new SmtpClient())
            {
                client.Connect(ExternalProvider.SMTP_Server, ExternalProvider.SMTP_Port);
                client.Authenticate(ExternalProvider.Forum_Mail, ExternalProvider.Forum_Mail_Password);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
            return new InfoResponse() { IsSuccess = true, Message = ResponseMessages.Send_Mail_Succeed };
        }
    }
}
