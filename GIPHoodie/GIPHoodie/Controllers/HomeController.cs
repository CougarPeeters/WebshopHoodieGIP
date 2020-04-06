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

namespace GIPHoodie.Controllers
{
    public class HomeController : Controller
    {
        PersistenceCode persistenceCode = new PersistenceCode();

        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("KlantID", 1);
            ArtikelRepository artikelRepo = new ArtikelRepository();
            artikelRepo.Artikels = persistenceCode.loadArtikels();
            return View(artikelRepo);
        }

        [HttpPost]

        public IActionResult Index(bool noUse)
        {
            return RedirectToAction("Winkelmand");
        }


        public IActionResult Toevoegen(int ArtID)
        {
            HttpContext.Session.SetInt32("ArtikelNr", ArtID);
            Artikel GeselecteerdeArtikel = persistenceCode.loadArtikel(ArtID);
            VMArtikelAantal vmArtikelAantal = new VMArtikelAantal();
            vmArtikelAantal.GeselecteerdArtikel = GeselecteerdeArtikel;

            return View(vmArtikelAantal);
        }

        [HttpPost]
        public IActionResult Toevoegen(VMArtikelAantal vMArtikelAantal)
        {
            WinkelmandItem winkelmand = new WinkelmandItem();

            winkelmand.ArtikelNr = Convert.ToInt32(HttpContext.Session.GetInt32("ArtikelNr"));
            winkelmand.KlantNr = Convert.ToInt32(HttpContext.Session.GetInt32("KlantID"));
            winkelmand.Aantal = vMArtikelAantal.Aantal;

            persistenceCode.PasMandAan(winkelmand);
            return RedirectToAction("Winkelmand", winkelmand);
        }

        public IActionResult Winkelmand()
        {
            VMWinkelmand vmWinkelmand = new VMWinkelmand();
            int klantid = Convert.ToInt32(HttpContext.Session.GetInt32("KlantID"));
            vmWinkelmand.klant = persistenceCode.KlantOphalen(Convert.ToInt32(klantid));
            
            
            if(persistenceCode.MandChecken(klantid) == true)
            {
                ViewBag.Controleer = true;
                WinkelmandRepository winkelmandRepository = new WinkelmandRepository();
                winkelmandRepository.winkelmandItems = persistenceCode.MandOphalen();
                vmWinkelmand.winkelRepository = winkelmandRepository;
                vmWinkelmand.totaal = persistenceCode.BerekenTotaal();
                return View(vmWinkelmand);
            }
            else
            {
                ViewBag.Controleer = false; 
                
                return View(vmWinkelmand);
            }
         

            
        }

        public IActionResult Verwijderen(int ArtNr)
        {
            WinkelmandItem winkelmand = new WinkelmandItem();

            winkelmand.ArtikelNr = ArtNr;
            winkelmand.KlantNr = Convert.ToInt32(HttpContext.Session.GetInt32("KlantID"));
            winkelmand.Aantal = persistenceCode.AantalOphalen(winkelmand);  

            persistenceCode.Verwijder(winkelmand);
            return RedirectToAction("Winkelmand");
        }

    }
}
