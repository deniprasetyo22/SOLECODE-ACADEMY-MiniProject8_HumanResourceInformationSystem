using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MiniProject7.Application.Interfaces.IServices;
using MiniProject7.Domain.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject7.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        private MimeMessage CreateEmailMessage(MailData mailData)
        {
            MimeMessage emailMessage = new MimeMessage();

            MailboxAddress emailFrom = new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId);

            emailMessage.From.Add(emailFrom);

            if (mailData.EmailToIds != null && mailData.EmailToIds.Any())
            {
                foreach (var to in mailData.EmailToIds)
                {
                    MailboxAddress emailTo = new MailboxAddress(to, to);
                    emailMessage.To.Add(emailTo);
                }
            }

            if (mailData.EmailCCIds != null && mailData.EmailCCIds.Any())
            {
                foreach (var cc in mailData.EmailCCIds)
                {
                    MailboxAddress emailCc = new MailboxAddress(cc, cc);
                    emailMessage.Cc.Add(emailCc);
                }
            }

            emailMessage.Subject = mailData.EmailSubject;

            BodyBuilder emailBodyBuilder = new BodyBuilder
            {
                HtmlBody = mailData.EmailBody
            };

            if (mailData.Attachments != null && mailData.Attachments.Any())
            {
                foreach (var file in mailData.Attachments)
                {
                    emailBodyBuilder.Attachments.Add(file);
                }
            }

            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private bool Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_mailSettings.Host, _mailSettings.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    client.Send(mailMessage);
                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }


        public bool SendMail(MailData mailData)
        {
            var emailMessage = CreateEmailMessage(mailData);
            var result = Send(emailMessage);
            return result;
        }
    }
}
