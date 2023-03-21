using System.Net;
using System.Net.Mail;
using System.Text;

namespace ArtBiathlon.Models
{
    public class MailSender
    {
        private SmtpClient _smtpClient;

        private MailMessage _mailMessage;

        private string _emailFrom, _emailTo, _displayName;

        const int _port = 587; // for mail.ru

        #region Secret

        private string _password = "BJrB20ztQPDmBRyZ88eH";

        #endregion

        private List<string> _mailDomains = new List<string>
        {
            "smtp.inbox.ru",
            "smtp.list.ru",
            "smtp.bk.ru",
            "smtp.yandex.ru",
            "smtp.mail.ru",
            "smtp.gmail.com"
        };

        public MailSender(string emailFrom, string emailTo, string displayName)
        {
            _emailFrom = emailFrom;
            _emailTo = emailTo;
            _displayName = displayName;

            var domain = GetDomain(emailFrom);

            _smtpClient = new SmtpClient(domain, _port);
            _smtpClient.Credentials = new NetworkCredential(_emailFrom, _password);
            _smtpClient.EnableSsl = true;
            _mailMessage = new MailMessage(new MailAddress(emailFrom), new MailAddress(emailTo));
            _mailMessage.SubjectEncoding = Encoding.UTF8;
            _mailMessage.BodyEncoding = Encoding.UTF8;
        }

        public string GetDomain(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new Exception();
                }
                else
                {
                    var mail = email.Split('@')[1];

                    return _mailDomains.FirstOrDefault(domain => domain.Contains(mail));
                }
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task Send(string subject, string message)
        {
            try
            {
                if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
                {
                    throw new Exception();
                }

                await Task.Run(() =>
                {
                    _mailMessage.Subject = subject;
                    _mailMessage.Body = message;
                    _smtpClient.Send(_mailMessage);
                });
            }
            catch(Exception)
            {

            }
        }
    }
}
