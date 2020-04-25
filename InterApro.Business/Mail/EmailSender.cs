using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InterApro.Models.Mail;

namespace InterApro.Business.Mail
{
    /// <summary>
    /// Implementacion de envio de correos
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        /// <summary>
        /// Envio de correo sincrono
        /// </summary>
        /// <param name="message">Mensaje a enviar</param>
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        /// <summary>
        /// Mensaje de correo asincrono
        /// </summary>
        /// <param name="message">Mensaje a enviar</param>
        /// <returns></returns>
        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        /// <summary>
        /// Metodo para crear los objetos de correo a enviar
        /// </summary>
        /// <param name="message">Mensaje a enviar</param>
        /// <returns></returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            if(message.EmailType != EmailTypes.None)
            {
                PopulateTemplate(message);
            }

            var bodyBuilder = new BodyBuilder { HtmlBody = message.Content};

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        /// <summary>
        /// Metodo para enviar el correo al servidor
        /// </summary>
        /// <param name="mailMessage">Objeto de mensaje nativo SMTP</param>
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }


        /// <summary>
        /// Metodo para enviar el correo de manera asincrona, no se espera a que el sevidor envie el correo de inmediato
        /// </summary>
        /// <param name="mailMessage">Objeto de mensaje nativo SMTP</param>
        /// <returns></returns>
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Metodo para obtener las plantilla del correo segun el tipo
        /// </summary>
        /// <param name="message">Mensaje a utilizar en la plantilla</param>
        private void PopulateTemplate(Message message)
        {
            string templatePath = _emailConfig.Templates[(int)message.EmailType - 1];

            string templateContent = "";
            using (StreamReader SourceReader = File.OpenText(templatePath))
            {
                templateContent = SourceReader.ReadToEnd();
            }
            message.Content = string.Format(templateContent, message.ContentTokens);
        }
    }
}
