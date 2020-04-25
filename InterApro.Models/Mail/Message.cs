using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterApro.Models.Mail
{
    /// <summary>
    /// Modelo del mensaje a enviar 
    /// </summary>
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public string[] ContentTokens { get; set; }

        public IFormFileCollection Attachments { get; set; }

        public EmailTypes EmailType { get; set; }

        public Message(IEnumerable<string> to, string subject, string content, EmailTypes emailType = EmailTypes.None, string[] contentTokens = null, IFormFileCollection attachments = null)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
            ContentTokens = contentTokens;
            Attachments = attachments;
            EmailType = emailType;
        }
    }
}
