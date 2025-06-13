using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace Medicare_API.Models
{
    public class EmailSender : IEmailSender
{
    private readonly string smtpHost = "smtp.gmail.com";
    private readonly int smtpPort = 587;
    private readonly string smtpUser = "medicare.tcc@gmail.com";       // Altere aqui
    private readonly string smtpPass = "mxgn krcc rzec soar";         // Altere aqui

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUser),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage);
    }
}
}