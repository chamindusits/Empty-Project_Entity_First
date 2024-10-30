using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SITS.BNS.Common
{
    public class EmailHelper
    {

        private string _host;
        private string _from;
        private string _alias;
        private string _password;
        private string _port;
        //private readonly IHttpContextAccessor _httpcontext;
        private bool _emailNotifications;

        public EmailHelper(IConfiguration iConfiguration)
        {

            var smtpSection = iConfiguration.GetSection("SMTP");
            if (smtpSection != null)
            {
                _host = smtpSection.GetSection("Host").Value;
                _from = smtpSection.GetSection("From").Value;
                _password = smtpSection.GetSection("Password").Value;
                _port = smtpSection.GetSection("Port").Value;
                _emailNotifications = Convert.ToBoolean(smtpSection.GetSection("EmailNotifications").Value);
            }
        }

        public void SendEmail(string to, string subject, string body)
        {

            try
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress(to));
                message.From = new MailAddress(_from, _alias);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                var client = new SmtpClient(_host);
                client.Port = Convert.ToInt32(_port);
                client.Credentials = new NetworkCredential(_from, _password);
                client.EnableSsl = true;
                ThreadPool.QueueUserWorkItem(state => client.Send(message));
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }
    }
}
