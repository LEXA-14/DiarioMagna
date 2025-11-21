using DiarioMagna.Domain.Models;

namespace DiarioMagna.Services.Email;
public interface INoticieroEmailService
{
    Task SendArticleAsync(string toEmail, Article article, string? attachmentPath = null);
}
