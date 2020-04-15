using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIPHoodie.Models
{
    public class MailZender
    {

        public Klant klant { get; set; }

        public void StuurMail()
        {
            string email = klant.Mail;
            string naam = klant.Naam;
            string Onderwerp = "Hoodieshop bevestigingsmail";
            string Bericht= "Beste, Hoodieshop heeft de bestelling ontvangen en zal je pakketje versturen zodra je hebt " +
                "overgeschreven naar de rekeningnummer rekeningnummer BE32 1214 4389 1894.";
            
            SendGridClient client = new SendGridClient(Environment.GetEnvironmentVariable("MijnAPI"));
            EmailAddress from = new EmailAddress("GIPHoodieShop@gmail.com", "HoodieShop");
            EmailAddress to = new EmailAddress(email, naam);
            client.SendEmailAsync(MailHelper.CreateSingleEmail(from, to, Onderwerp, Bericht, ""));
                
            

        }
    }
}
