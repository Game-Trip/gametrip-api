using MimeKit;

namespace GameTrip.Provider.IProvider
{
    public interface IEmailProvider
    {
        public void SendMail(MimeMessage email);
    }
}
