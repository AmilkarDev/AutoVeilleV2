using Autoveille.Models;
using AutoveilleBL;
using AutoveilleBL.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Autoveille.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["Commerces"] == null)
            {
                return RedirectToAction("Connexion", "Compte");
            }
            List<UtilisateurSiteCommerces> commerces = (List<UtilisateurSiteCommerces>)Session["Commerces"];
            var concessionsModel = new ConcessionsModel()
            {
                ListConcessions = commerces,
            };
            return View(concessionsModel);
        }

        public JsonResult TestJson(int aId)
        {
            List<UtilisateurSiteCommerces> commerces = (List<UtilisateurSiteCommerces>)Session["Commerces"];
            return Json(commerces, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InfoConcession(int aNoCommerce)
        {
            int noCommerce;
            if (Session["User"] != null )
            {
                var role= Utilisateurs.GetRoles(Session["User"].ToString(), aNoCommerce);
                if (role != null)
                {
                    Session["NoCommerce"] = aNoCommerce;
                    Session["Role"] = role.Role;
                }
                else
                {
                    return RedirectToAction("Connexion", "Compte");
                }
            }
            else
            {
                return RedirectToAction("Connexion", "Compte");
            }
            //var ventes = Ventes.GetVentes(aNoCommerce);
            var appels = new Appels();
            appels.Evenement = Ventes.GetProchaineEvenement(aNoCommerce);
            appels.Relances = Ventes.GetRelances(appels.Evenement.IdEvenement, aNoCommerce);
            return View();
        }
        

    }
}
