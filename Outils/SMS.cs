using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml.Serialization;
using Twilio;
using RestSharp;

namespace Outils
{
  
    public class SMS
    {
        public String To { get; set; }
        public String From { get; set; }
        public String Mess { get; set; }
        private  const string _accountSid = "ACd5ecb1e682694349bc89e13ea2d283c5";
        private const string _authToken = "9b74005da6f74663690330a5b4078785";

        public void SendSMS()
        {
            try
            {
                var twilio = new TwilioRestClient(_accountSid, _authToken);
                var message = twilio.SendMessage(From, To, Mess, "");   
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed to send SMS: {0}", ex.Message);
                throw new ReadableException("Une erreure c'est produit lors de l'envois d'un SMS.", ex);
            }
        }

 
    }
}
