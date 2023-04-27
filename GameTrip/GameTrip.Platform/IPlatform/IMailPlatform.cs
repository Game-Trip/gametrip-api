using GameTrip.Domain.Models.Email;

namespace GameTrip.Platform.IPlatform;

public interface IMailPlatform
{
    public Task SendMailAsync(MailDTO mailData);
}