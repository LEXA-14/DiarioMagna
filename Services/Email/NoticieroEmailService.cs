using DiarioMagna.Domain.Models;
using DiarioMagna.Services.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Runtime;

namespace DiarioMagna.Services.Email;
public class NoticieroEmailService : INoticieroEmailService
{
    private readonly EmailSettings _settings;
    private readonly IWebHostEnvironment _env;

    public NoticieroEmailService(IOptions<EmailSettings> settings, IWebHostEnvironment env)
    {
        _settings = settings.Value;
        _env = env;
    }

    public async Task SendArticleAsync(string toEmail, Article article, string? attachmentPath = null)
    {
        var subject = $"Nuevo artículo enviado: {article.Title}";

        var body = $@"
                <p>Estimado equipo,</p>
                <p>Se les informa que se ha enviado un nuevo artículo:</p>
                <p><strong>Título:</strong> {article.Title}</p>
                <p><strong>Autor:</strong> {article.AuthorName}</p>
                <p><strong>Categoría:</strong> {article.Category.Name}</p>
                <p><strong>Fecha:</strong> {article.CreatedAt:dd/MM/yyyy HH:mm}</p>
                <p>{article.Content}</p>
                {(string.IsNullOrEmpty(attachmentPath) ? "" : "<p>Se adjunta un archivo.</p>")}
                <p>Gracias,<br><strong>Diario Magna</strong></p>
                ";
        using var client = new SmtpClient(_settings.SmtpServer)
        {
            Port = _settings.Port,
            Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(toEmail);

        if (!string.IsNullOrEmpty(attachmentPath))
        {
            var relativePath = attachmentPath.TrimStart('/');
            var fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (File.Exists(fullPath))
            {
                mail.Attachments.Add(new Attachment(fullPath));
            }
            else
            {
                Console.WriteLine($"Archivo adjunto no encontrado: {fullPath}");
            }
        }   
        await client.SendMailAsync(mail);
    }
}
