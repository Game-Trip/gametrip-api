using GameTrip.Domain.Entities;
using GameTrip.Domain.Models.Email;
using GameTrip.Domain.Settings;
using GameTrip.Platform.IPlatform;
using GameTrip.Provider.IProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace GameTrip.Platform
{
    public class MailPlatform : IMailPlatform
    {
        #region Properties
        private readonly IEmailProvider _emailProvider;
        private readonly MailSettings _mailSettings;


        #endregion Properties

        #region Constructor

        public MailPlatform(IEmailProvider emailProvider, MailSettings mailSettings)
        {
            _emailProvider = emailProvider;
            _mailSettings = mailSettings;
        }

        #endregion Constructor

        #region Public Methods
        public async Task SendMailAsync(MailDTO mailData)
        {
            MimeMessage email = new();
            MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderMail);
            email.From.Add(emailFrom);
            MailboxAddress emailTo = new MailboxAddress(mailData.Name, mailData.Email);
            email.To.Add(emailTo);

            email.Subject = mailData.Subject;

            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = mailData.Body;

            email.Body = emailBodyBuilder.ToMessageBody();

            await _emailProvider.SendMailAsync(email);
        }



        #endregion Public Methods
    }
}