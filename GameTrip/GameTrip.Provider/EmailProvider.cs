using GameTrip.Domain.Models.Email.Template;
using GameTrip.Domain.Settings;
using GameTrip.Provider.IProvider;
using MailKit.Net.Smtp;
using MimeKit;

namespace GameTrip.Provider;

public class EmailProvider : IEmailProvider
{
    private readonly MailSettings _mailSettings;

    public EmailProvider(MailSettings mailSettings)
    {
        _mailSettings = mailSettings;
    }


    public async Task SendMailAsync(MimeMessage email)
    {
        using (SmtpClient smtpClient = new())
        {
            await smtpClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
            await smtpClient.SendAsync(email);
            await smtpClient.DisconnectAsync(true);
            smtpClient.Dispose();
        }
    }
    public string? GetTemplate(TemplatePath path)
    {
        string filePath = Directory.GetCurrentDirectory() + @$"/models/Template/{path}";
        if (File.Exists(filePath)) return File.ReadAllText(filePath);
        else return null;
    }
}
