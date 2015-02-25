using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using WebApplication1.Models;

namespace WebApplication1.BusinessLogic
{
    public class MailHelper
    {
        public const string SUCCESS
        = "Success! Your email has been sent.  Please allow up to 48 hrs for a reply.";
        //string to = "stephe_e@hotmail.com"; // Specify where you want this email sent.
        // This value may/may not be constant.
        // To get started use one of your email 
        // addresses.
        public string EmailFromArvixe(Message message)
        {

            // Use credentials of the Mail account that you created with the steps above.
            const string FROM = "stephen@stephenleongportfolio.com";
            const string FROM_PWD = "password";
            const bool USE_HTML = true;

            // Get the mail server obtained in the steps described above.
            const string SMTP_SERVER = "mail.stephenleongportfolio.com.BROWN.mysitehosted.com";
            try
            {
                MailMessage mailMsg = new MailMessage(FROM, message.SendToEmail);
                mailMsg.Subject = message.Subject;
                mailMsg.Body = "Sent by: " + message.Sender + " From: " + message.SenderCompany + "<br/>Email reply to: " + message.SenderEmail + "<br/><br/>Message:<br/>" + message.Body;
                mailMsg.IsBodyHtml = USE_HTML;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 25;
                smtp.Host = SMTP_SERVER;
                smtp.Credentials = new System.Net.NetworkCredential(FROM, FROM_PWD);
                smtp.Send(mailMsg);
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            return SUCCESS;
        }
    }
}