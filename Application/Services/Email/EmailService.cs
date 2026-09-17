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

        var resetUrl =
            $"https://marcelo-doceiro.vercel.app/reset-password" +
            $"?email={Uri.EscapeDataString(email)}" +
            $"&token={Uri.EscapeDataString(token)}";

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(
                smtpEmail,
                smtpPassword
            )
        };

        var message = new MailMessage
        {
            From = new MailAddress(smtpEmail!),
            Subject = "Redefinição de senha",
            Body = $"""
            <html>
                <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333;">

                    <h2>Redefinição de senha</h2>

                    <p>Olá! Recebemos uma solicitação para redefinir sua senha.</p>

                    <p>
                        Clique no botão abaixo para criar uma nova senha:
                    </p>

                    <p>
                        <a href="{resetUrl}"
                           style="
                               display: inline-block;
                               padding: 12px 24px;
                               background-color: #f7b6c8;
                               color: white;
                               text-decoration: none;
                               border-radius: 8px;
                               font-weight: bold;
                           ">
                            Redefinir minha senha
                        </a>
                    </p>

                    <p>
                        Este link é válido por <strong>30 minutos</strong>.
                    </p>

                    <p>
                        Caso você não tenha solicitado a redefinição de senha,
                        ignore este e-mail.
                    </p>

                </body>
            </html>
            """,
            IsBodyHtml = true
        };

        message.To.Add(email);

        await client.SendMailAsync(message);
    }
}