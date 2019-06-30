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
        public static void Send(string emailAddress)
        {
            MailAddress From = new MailAddress("filter.kvartir@yandex.ru", "Фильтр Квартир");
            MailAddress To = new MailAddress(emailAddress);
            MailMessage message = new MailMessage(From, To);
            message.Subject = "Рассылка от приложения \"Фильтр квартир\" ";
            message.Body = "Здравствуйте! Самые свежие предложения квартир с популярных сайтов уже собраны для Вас! Хорошего дня!  ";

            bool isNext = false;
            while (!isNext)
            {
                try {
                    message.Attachments.Add(new Attachment(Directory.GetCurrentDirectory() + "/table.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                    isNext = true;
                } catch (Exception) { }
            }
            message.IsBodyHtml = false;

            string smtpHost = "smtp.yandex.ru";
            int smtpPort = 587;
            SmtpClient client = new SmtpClient(smtpHost, smtpPort);

            string login = "filter.kvartir@yandex.ru";
            string password = "apogdtcttpjrzjfp";
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(login, password);

            client.EnableSsl = true;
            client.SendMailAsync(message);
        }
    }
}