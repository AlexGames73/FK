using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace fk
{
    public class EmailSender
    {
        public static void Send(string emailAddress, byte[] message)
        {
            MailAddress From = new MailAddress("filter.kvartir@yandex.ru", "Фильтр Квартир");
            MailAddress To = new MailAddress(emailAddress);
            MailMessage Message = new MailMessage(From, To);
            Message.Subject = "Рассылка от приложения \"Фильтр квартир\" ";
            Message.Body = "Здравствуйте! Самые свежие предложения квартир с популярных сайтов уже собраны для Вас! Хорошего дня!  ";

            bool isNext = false;
            while (!isNext)
            {
                try {
                    Message.Attachments.Add(new Attachment(new MemoryStream(message), "Список квартир.xlsx"));
                    isNext = true;
                } catch (Exception) { }
            }
            Message.IsBodyHtml = false;

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