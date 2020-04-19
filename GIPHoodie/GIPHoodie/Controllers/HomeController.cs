using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GIPHoodie.Models;
using GIPHoodie.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace GIPHoodie.Controllers
{
    public class HomeController : Controller
    {
        PersistenceCode persistenceCode = new PersistenceCode();


        [Authorize]
        public IActionResult Index()
        {


            ArtikelRepository artikelRepo = new ArtikelRepository();
            artikelRepo.Artikels = persistenceCode.loadArtikels();
            return View(artikelRepo);
        }
        [Authorize]
        [HttpPost]

        public IActionResult Index(bool noUse)
        {

            return RedirectToAction("Winkelmand");
        }

        [Authorize]
        public IActionResult Toevoegen(int ArtID)
        {
            HttpContext.Session.SetInt32("ArtikelNr", ArtID);
            Artikel GeselecteerdeArtikel = persistenceCode.loadArtikel(ArtID);
            VMToevoegen vmToevoegen = new VMToevoegen();
            vmToevoegen.GeselecteerdArtikel = GeselecteerdeArtikel;

            return View(vmToevoegen);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Toevoegen(VMToevoegen vmToevoegen)
        {
            WinkelmandItem winkelmand = new WinkelmandItem();
            Artikel art = new Artikel();
            vmToevoegen.GeselecteerdArtikel = persistenceCode.loadArtikel(Convert.ToInt32(HttpContext.Session.GetInt32("ArtikelNr")));
            vmToevoegen.invoer = Convert.ToString(vmToevoegen.Aantal);
            art.Voorraad = persistenceCode.VoorraadOphalen(Convert.ToInt32(HttpContext.Session.GetInt32("ArtikelNr")));
            if (ModelState.IsValid)
            {

                winkelmand.ArtikelNr = Convert.ToInt32(HttpContext.Session.GetInt32("ArtikelNr"));
                winkelmand.KlantNr = Convert.ToInt32(HttpContext.Session.GetInt32("KlantID"));
                winkelmand.Aantal = Convert.ToInt32(vmToevoegen.invoer);
                HttpContext.Session.SetString("Aantal", Convert.ToString(vmToevoegen.Aantal));


                if (int.TryParse(vmToevoegen.invoer, out int aantal))
                {

                    if (aantal < 1)
                    {
                        ViewBag.FoutToevoegen = "Je moet minstens 1 item bestellen";
                        return View(vmToevoegen);
                    }
                    else if (aantal > art.Voorraad)
                    {
                        ViewBag.FoutToevoegen = "Je kan niet meer bestellen dan wij in voorraad hebben";
                        return View(vmToevoegen);
                    }
                    else
                    {
                        persistenceCode.PasMandAan(winkelmand);
                        return RedirectToAction("Winkelmand", winkelmand);
                    }
                }
                else
                {
                    vmToevoegen.Aantal = aantal;
                    ViewBag.FoutToevoegen = "Je moet een geheel ingeven";
                    return View(vmToevoegen);
                }


            }
            else
            {
                return View(vmToevoegen);
            }



        }
        [Authorize]
        public IActionResult Winkelmand()
        {
            VMWinkelmand vmWinkelmand = new VMWinkelmand();
            int klantid = Convert.ToInt32(HttpContext.Session.GetInt32("KlantID"));
            vmWinkelmand.klant = persistenceCode.KlantOphalen(Convert.ToInt32(klantid));
            
            
            if(persistenceCode.MandChecken(klantid) == true)
            {
                ViewBag.Controleer = true;

                WinkelmandRepository winkelmandRepository = new WinkelmandRepository();
                  
                winkelmandRepository.winkelmandItems = persistenceCode.MandOphalen(klantid);
                vmWinkelmand.winkelRepository = winkelmandRepository;
                vmWinkelmand.totaal = persistenceCode.BerekenTotaal(klantid);
                HttpContext.Session.SetString("Totaal", Convert.ToString(vmWinkelmand.totaal.TotaalIncl));

                return View(vmWinkelmand);
            }
            else
            {
                ViewBag.Controleer = false; 
                
                return View(vmWinkelmand);
            }
         

            
        }
        [Authorize]
        [HttpPost]
        public IActionResult Winkelmand(VMBestelling vmBestelling)
        {
            int klantID = Convert.ToInt32(HttpContext.Session.GetInt32("KlantID"));
            vmBestelling.klant = persistenceCode.KlantOphalen(klantID);
            vmBestelling.Bestelling = persistenceCode.Bevestigen(klantID);
            double TotaalIncl = Convert.ToDouble(HttpContext.Session.GetString("Totaal"));
            ViewBag.TotaalIncl = TotaalIncl;
            MailZender mailZender = new MailZender();
            mailZender.klant=persistenceCode.KlantOphalen(klantID);
            mailZender.StuurMail();
            return RedirectToAction("Bevestiging");
        }

        [Authorize]
        public IActionResult Verwijderen(int ArtNr)
        {
            WinkelmandItem winkelmand = new WinkelmandItem();

            winkelmand.ArtikelNr = ArtNr;
            winkelmand.KlantNr = Convert.ToInt32(HttpContext.Session.GetInt32("KlantID"));
            winkelmand.Aantal = persistenceCode.AantalOphalen(winkelmand);  

            persistenceCode.Verwijder(winkelmand);
            return RedirectToAction("Winkelmand");
        }
        [Authorize]
        public IActionResult Bevestiging(VMBestelling vmBestelling)
        {
            int klantID = Convert.ToInt32(HttpContext.Session.GetInt32("KlantID"));
            vmBestelling.klant = persistenceCode.KlantOphalen(klantID);
            vmBestelling.Bestelling = persistenceCode.Bevestigen(klantID);
            double prijs = Convert.ToDouble(HttpContext.Session.GetString("Totaal"));
            ViewBag.prijs = prijs;


            return View(vmBestelling);
        }

       

    }
}
