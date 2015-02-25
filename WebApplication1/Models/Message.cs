using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Message
    {
        public string Sender { get; set; }
        public string SenderCompany { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SendToEmail { get; set; }

        public Message() { }
        public Message(string sender, string senderCompany, string senderEmail, string subject, string body, string sendToEmail)
        {
            Sender = sender;
            SenderCompany = senderCompany;
            SenderEmail = senderEmail;
            Subject = subject;
            Body = body;
            SendToEmail = sendToEmail;
        }

    }
}