using DiarioMagna.Domain.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

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
        var safeContent = WebUtility.HtmlEncode(article.Content);

        string imageTagHtml = "";
        Attachment? inlineImage = null;


        if (!string.IsNullOrWhiteSpace(article.UploadedFilePath))
        {
            try
            {
                var relativeImagePath = article.UploadedFilePath.TrimStart('/').Replace("\\", "/");
                var fullImagePath = Path.Combine(_env.WebRootPath, relativeImagePath);

                if (File.Exists(fullImagePath))
                {
                    inlineImage = new Attachment(fullImagePath);
                    inlineImage.ContentId = "imageArticle";
                    inlineImage.ContentDisposition!.Inline = true;
                    inlineImage.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                    imageTagHtml = $@"
                        <p><strong>Imagen del artículo:</strong></p>
                        <img src='cid:imageArticle' 
                             alt='Imagen del artículo'
                             style='max-width:600px;width:100%;height:auto;border-radius:8px;margin-top:10px; display:block;' />";
                }
                else
                {
                    Console.WriteLine($"Imagen no encontrada en disco: {fullImagePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error preparando imagen incrustada: {ex.Message}");
            }
        }

        var htmlBody = $@"
            <html>
            <body style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>
                <p>Estimado equipo,</p>
                <p>Se les informa que se ha enviado un nuevo artículo:</p>
                <div style='background-color: #f9f9f9; padding: 15px; border-radius: 5px; border: 1px solid #eee;'>
                    <p><strong>Título:</strong> {article.Title}</p>
                    <p><strong>Autor:</strong> {article.AuthorName}</p>
                    <p><strong>Categoría:</strong> {article.Category?.Name ?? "N/A"}</p>
                    <p><strong>Fecha:</strong> {article.CreatedAt:dd/MM/yyyy HH:mm}</p>
                </div>
        
                {imageTagHtml}

                <div style='margin-top: 20px; white-space: pre-wrap;'>{safeContent}</div>

                <p style='margin-top: 25px; border-top: 1px solid #ddd; padding-top: 10px;'>
                    Gracias,<br>
                    <strong>Diario Magna</strong>
                </p>
            </body>
            </html>";

        using var client = new SmtpClient(_settings.SmtpServer)
        {
            Port = _settings.Port,
            Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        using var mail = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        mail.To.Add(toEmail);

        if (inlineImage != null)
        {
            mail.Attachments.Add(inlineImage);
        }

        if (!string.IsNullOrWhiteSpace(attachmentPath))
        {
            try
            {
                var relativePath = attachmentPath.TrimStart('/').Replace("\\", "/");
                var fullPath = Path.Combine(_env.WebRootPath, relativePath);

                if (File.Exists(fullPath))
                {
                    var attachment = new Attachment(fullPath);
                    attachment.ContentDisposition!.Inline = false; 
                    mail.Attachments.Add(attachment);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al adjuntar archivo: {ex.Message}");
            }
        }

        try
        {
            await client.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error enviando correo: {ex.Message}");
        }
    }
}