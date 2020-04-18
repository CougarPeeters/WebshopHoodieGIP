using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GIPHoodie.Models;
using GIPHoodie.Persistence;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace GIPHoodie.Controllers
{
    public class AuthController : Controller
    {
        PersistenceCode persistenceCode = new PersistenceCode();

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
           
            return View();
        }

        [HttpPost]

        public IActionResult Login(string returnUrl, Klant klantCr)
        {
            if (ModelState.IsValid)
            {

                if (persistenceCode.CheckCredentials(klantCr) != -1)
                {

                    HttpContext.Session.SetInt32("KlantID", Convert.ToInt32(persistenceCode.CheckCredentials(klantCr)));
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, klantCr.Naam)
                };
                    var userIdentity = new ClaimsIdentity(claims, "SecureLogin");
                    var userPrincipal = new ClaimsPrincipal(userIdentity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.Today.AddDays(1),
                            IsPersistent = false,
                            AllowRefresh = false
                        });
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.fout = "Ongeldige inlog. Probeer opniew.";
                    return View();
                }
            }
            else
            {
                return View();
            }

        }
    }
}