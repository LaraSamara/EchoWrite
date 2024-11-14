using Blog.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Blog.PL.Helpers
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                 EnableSsl = true,
                 Credentials = new NetworkCredential("larasamara2002@gmail.com", "sddr rjxw jqdi umnr"),
            };


            // Create a new MailMessage object
            var mailMessage = new MailMessage
            {
                From = new MailAddress("larasamara2002@gmail.com", "EchoWrite"), // Specify the display name here
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true // Set to true if you're sending HTML formatted email
            };

            mailMessage.To.Add(email.To); // Add the recipient's email address

            // Send the email
            client.Send(mailMessage);
        }
    }
}
