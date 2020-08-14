using AssetsMonitor.Domain.Messager;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AssetsMonitor.Services.Messager
{
    class EmailMessager : IMessager
    {
        private EmailConfiguration emailConfiguration;
        public void configure(EmailConfiguration conf)
        {
            if (conf is null)
                throw new ArgumentNullException("Configuração deve estar definida");
            if (string.IsNullOrEmpty(conf.Domain)
                || string.IsNullOrEmpty(conf.FromEmail)
                || string.IsNullOrEmpty(conf.Password)
                || string.IsNullOrEmpty(conf.Username)
                || conf.Port == 0)
                throw new ArgumentException("Nenhuma configuração de SMTP deve ser nula ou vazia");
            this.emailConfiguration = conf;
        }

        public async Task sendAsync(string message, string subject)
        {
            try
            {
                string toEmail = this.emailConfiguration.ToEmailList[0];
                MailAddress mailAddress = null;
                if (this.emailConfiguration.DisplayName is null)
                    mailAddress = new MailAddress(this.emailConfiguration.FromEmail);
                else
                    mailAddress = new MailAddress(this.emailConfiguration.FromEmail, this.emailConfiguration.DisplayName);

                MailMessage mail = new MailMessage()
                {
                    From = mailAddress
                };

                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = "AssetMonitor - " + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(this.emailConfiguration.Domain, this.emailConfiguration.Port))
                {
                    smtp.Credentials = new NetworkCredential(this.emailConfiguration.Username, this.emailConfiguration.Password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
