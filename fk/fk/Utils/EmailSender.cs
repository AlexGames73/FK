using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace fk.Utils
{
    public class EmailSender
    {
        private static Dictionary<MessageType, (string, string)> Messages = new Dictionary<MessageType, (string, string)>()
        {
            { MessageType.Mailing, (
                "Рассылка приложения \"Фильтр квартир\"",

                "<h1 style=\"text-align=center;\">Самые свежие предложения квартир с популярных сайтов уже собраны для Вас!</h1>"
            ) },

            { MessageType.Activation, (
                "Код активации приложения \"Фильтр квартир\"",

                "<p>Код активации приложения.</p>" + "<br/><br/>" +
                "<h1>{0}</h1>" + "<br/><br/>" +
                "<p>Никому не сообщайте этот код в целях вашей же безопасности.</p>"
            ) }
        };

        public enum MessageType
        {
            Mailing, Activation
        }

        public static void Send(string emailAddress, MessageType messageType, params object[] parameters)
        {
            MailAddress From = new MailAddress("filter.kvartir@yandex.ru", "Фильтр Квартир");
            MailAddress To = new MailAddress(emailAddress);
            MailMessage Message = new MailMessage(From, To);
            Message.Subject = Messages[messageType].Item1;
            Message.Body = string.Format(Messages[messageType].Item2, parameters[0]);

            if (parameters[0].GetType() == typeof(byte[]))
                Message.Attachments.Add(new Attachment(new MemoryStream((byte[])parameters[0]), "Список квартир.xlsx"));
            Message.IsBodyHtml = true;

            string smtpHost = "smtp.yandex.ru";
            int smtpPort = 587;
            SmtpClient client = new SmtpClient(smtpHost, smtpPort);

            string login = "filter.kvartir@yandex.ru";
            string password = "apogdtcttpjrzjfp";
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(login, password);

            client.EnableSsl = true;
            client.SendMailAsync(Message);
        }
    }
}