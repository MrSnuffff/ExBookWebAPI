using System.Net;
using System.Net.Mail;

namespace ExBookWebAPI.Security
{
    public static class SendingEmailManager
    {
        // Sender's email credentials
        private const string SenderEmail = "noreplay.bookex@gmail.com";
        private const string SenderPassword = "rqeu cxvk iuiv uqsh";

        public static async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            // Create and configure the SMTP client
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                smtpClient.EnableSsl = true;

                // Create the email message
                using (MailMessage mailMessage = new MailMessage(SenderEmail, recipientEmail))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    try
                    {
                        // Send the email asynchronously
                        await smtpClient.SendMailAsync(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending email: {ex.Message}");
                    }
                }
            }
        }
    }
}
