using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace EventServer.Api.Services
{
    public class EmailService
    {
        public void Send(string to, string subject, string body)
        {
            MailMessage mailMsg = new MailMessage();

            // To
            mailMsg.To.Add(new MailAddress(to));

            // From
            mailMsg.From = new MailAddress("noreply@joinin.com", "JoinIn");

            // Subject and multipart/alternative Body
            mailMsg.Subject = subject;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Plain));
            //mailMsg.Attachments.Add(new Attachment(new MemoryStream(Attachment), AttachmentName));

            // Init SmtpClient and send
            var smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
            var credentials = new NetworkCredential("azure_92229888a847e114db064c535f5d6e4b@azure.com", "ipxspx123");
            smtpClient.Credentials = credentials;

            smtpClient.Send(mailMsg);
        }
    }
}
