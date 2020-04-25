using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InterApro.Models.Mail;

namespace InterApro.Business.Mail
{
    /// <summary>
    /// Interfaz para enviar correos electronicos
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Envio de correo sincrono.
        /// </summary>
        /// <param name="message">Mensaje a enviar</param>
        void SendEmail(Message message);

        /// <summary>
        /// Envio de correo Asincrono
        /// </summary>
        /// <param name="message">Mensaje a enviar</param>
        /// <returns></returns>
        Task SendEmailAsync(Message message);
    }
}
