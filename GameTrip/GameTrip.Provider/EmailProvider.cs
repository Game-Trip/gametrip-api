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

    public void SendMail(MimeMessage email)
    {
        using (SmtpClient smtpClient = new())
        {
            smtpClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            smtpClient.Send(email);
            smtpClient.Dispose();
        }
    }
}
