using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Net.Mime;
using System.Text;

namespace AppointmentScheduling.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            //using ethereal => go to https://ethereal.email/ to create account first

            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("doaaghaly397@gmail.com"));
            email.To.Add(MailboxAddress.Parse("doaaghaly397@gmail.com"));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = htmlMessage };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("tiana12@ethereal.email", "jnSPtabgmNAvUf8fjg");
            smtp.Send(email);
            smtp.Disconnect(true);
 

        }
    }
}
