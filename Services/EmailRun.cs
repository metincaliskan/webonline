using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebControlApp.Services
{
    public class EmailRun
    {
        public void SendEmail(string Email, string Ad, string Url)
        { 
            try
            {
                    var senderEmail = new MailAddress("test@gmail.com", "Metin");
                    var receiverEmail = new MailAddress(Email, "Receiver");
                    var password = "123123";
                    var sub = "Siteye Ulaşılamıyor";
                    var body = string.Format("{0} adlı {1} adresine ulaşılamıyor. Son kontrol Zamanı : {2}", Ad, Url, DateTime.Now);
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                
            }
            catch (Exception)
            {
            }
        }
    }
}
