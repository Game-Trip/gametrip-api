using GameTrip.Domain.Models.Email;

namespace GameTrip.Platform.IPlatform
{
    public interface IMailPlatform
    {
        public void SendMail(MailDTO mailData);
    }
}
