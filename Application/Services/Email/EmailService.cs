using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace ApiDoces.Services;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendPasswordResetEmail(string email, string token)
    {
        var smtpHost = configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(configuration["Email:SmtpPort"]!);
        var smtpEmail = configuration["Email:Email"];
        var smtpPassword = configuration["Email:Password"];

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(smtpEmail, smtpPassword)
        };

        var message = new MailMessage
        {
            From = new MailAddress(smtpEmail!),
            Subject = "Redefinição de senha",
            Body = $"""
                   Olá, Marcelo
                   Recebemos uma solicitação para redefinir sua senha.
                   Seu código de redefinição é:
                   {token}

                   Este código é válido por 30 minutos.
                   Caso você não tenha solicitado a redefinição de senha, ignore este e-mail.
                   """,
            IsBodyHtml = false
        };

        message.To.Add(email);
        await client.SendMailAsync(message);
    }
}