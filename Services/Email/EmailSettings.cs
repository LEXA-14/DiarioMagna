namespace DiarioMagna.Services.Email;
public class EmailSettings
{
    public string FromEmail { get; set; } = "";
    public string FromName { get; set; } = "";
    public string SmtpServer { get; set; } = "";
    public int Port { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public bool EnableSsl { get; set; } = true;
}
