
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NETCore.MailKit.Infrastructure.Internal;
using NETCore.MailKit.Core;
using MimeKit;

namespace MilanaBoutique.Helpers
{
    public class EmailService : IEmailService

    {
        public void Send(string mailTo, string subject, string message, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string subject, string message, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string mailCc, string mailBcc, string subject, string message, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public void Send(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string subject, string message, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string subject, string message, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(string mailTo, string mailCc, string mailBcc, string subject, string message, Encoding encoding, bool isHtml = false, SenderInfo sender = null)
        {
            throw new NotImplementedException();
        }


        public async Task SendEmailAsync(List<string> email, string subject, string message)

        {

            var emailMessage = new MimeMessage();




            emailMessage.From.Add(new MailboxAddress("Milana Boutique", "finalprojectcodeacademy@gmail.com"));

            foreach (var item in email)

            {

                emailMessage.To.Add(new MailboxAddress("", item));

            }

            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)

            {

                Text = message

            };


            using (var client = new SmtpClient())

            {

                await client.ConnectAsync("smtp.gmail.com", 587, true);

                await client.AuthenticateAsync("finalprojectcodeacademy@gmail.com", "shldhrtkghufzhmg");

                await client.SendAsync(emailMessage);




                await client.DisconnectAsync(true);

            }

        }

    }
}
