using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;

namespace Outils
{
    [Serializable()]
    public class Email
    {
        public List<String> To { get; set; }
        public List<String> BCC { get; set; }
        public List<String> CC { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public String ReplyTo { get; set; }
        public List<String> FichiersAtaches { get; set; } 

        public Email()
        {
            To = new List<string>();
            BCC  =  new List<string>();
            FichiersAtaches = new List<string>();
        }

        public String GetToListAsString()
        {
            return To!=null?To.Aggregate((c, n) => c + ";" + n):"";
        }
        public String GetCCListAsString()
        {
            return CC!=null && CC.Count()!=0?CC.Aggregate((c, n) => c + ";" + n):"";
        }
        public String GetBCCListAsString()
        {
            return BCC != null && BCC.Count() != 0 ? BCC.Aggregate((c, n) => c + ";" + n) : "";
        }

        public void SendEmail()
        {
            try
            {
                var smtpHost = ConfigurationManager.AppSettings["smtpHost"];
                if (String.IsNullOrEmpty(smtpHost))
                    throw new ReadableException("Il n'y a pas de SMTP host de configuré!");
                var port = 25;
                int.TryParse(ConfigurationManager.AppSettings["smtpHost"], out port);

                var message = new MailMessage();
                To.ForEach(x => message.To.Add(x));
                if( BCC != null )
                    BCC.ForEach(x => message.Bcc.Add(x));
                if (CC != null)
                    CC.ForEach(x => message.CC.Add(x));
                message.Subject = Subject;
                message.From = new MailAddress(ReplyTo);
                message.Body = Body;
                message.IsBodyHtml = Body.ToLower().Contains("<!doctype html");
                if (FichiersAtaches != null && FichiersAtaches.Count() > 0)
                {
                    foreach (var fa in FichiersAtaches)
                    {
                        message.Attachments.Add(new Attachment(fa));
                    }
                }
                var smtp = new SmtpClient(smtpHost, port);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to send email: {0}", ex.Message);
                throw new ReadableException("Une erreure c'est produit lors de l'envois d'un email.", ex);
            }
        }

        public String ToXml()
        {
            var serializer = new XmlSerializer(typeof(Email));

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);

                return writer.ToString();
            }
        }

        public static Email Desirealize(String aXml)
        {
            if (String.IsNullOrEmpty(aXml))
                return null;

            var ser = new XmlSerializer(typeof(Email));
            using (var reader = new StringReader(aXml))
            {
                return ser.Deserialize(reader) as Email;
            }
        }
    }
}
