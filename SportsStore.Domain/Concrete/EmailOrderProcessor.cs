using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using System.Net;
using System.Net.Mail;

namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "zamowienia@przyklad.pl";
        public string MailFromAddress = "sklepsportowy@przyklad.pl";
        public bool UseSsl = true;
        public string Username = "UzytkownikSmtp";
        public string Password = "HasloSmtp";
        public string ServerName = "smtp.przyklad.pl";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"C:\Users\maksi\Desktop\emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var SmtpClient = new SmtpClient())
            {
                SmtpClient.EnableSsl = emailSettings.UseSsl;
                SmtpClient.Host = emailSettings.ServerName;
                SmtpClient.Port = emailSettings.ServerPort;
                SmtpClient.UseDefaultCredentials = false;
                SmtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    SmtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    SmtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    SmtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder().AppendLine("Nowe zamówienie").AppendLine("---").AppendLine("Produkty:");

                foreach(var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (wartość: {2:c}", line.Quantity, line.Product.Name, subtotal);
                }

                try
                {
                    body.AppendFormat("Wartość całkowita: {0:x}", cart.ComputeTotalValue())
                    .AppendLine("---").
                    AppendLine("Wysyłka dla:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Street)
                    .AppendLine(shippingInfo.Number.ToString())
                    .AppendLine(shippingInfo.State)
                    .AppendLine(shippingInfo.State)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---");
                }
                catch (FormatException)
                {
                    
                }

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToAddress,
                    "Otrzymano nowe zamówienie!",
                    body.ToString());

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                SmtpClient.Send(mailMessage);
            }
        }
    }
}
