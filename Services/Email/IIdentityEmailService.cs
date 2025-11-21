namespace DiarioMagna.Services.Email;
public interface IIdentityEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
