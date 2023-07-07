using MimeKit;

namespace GameTrip.Provider.IProvider;

public interface IEmailProvider
{
    public Task SendMailAsync(MimeMessage email);

    string? GetTemplate(string path);
}