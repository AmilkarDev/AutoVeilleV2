using Autoveille.Models;
using Autoveille.TestingModels;
using AutoveilleBL;
using AutoveilleBL.Models.Web;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Autoveille.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult newDashboard()
        {
            return View();
        }
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

        public ActionResult Dashboard()
        {

            return View();
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



        //Data testing methods 

        public ActionResult GetNomAppels()
        {
            List<NomAppel> nomAppels = new List<NomAppel>
            {
                new NomAppel{Id=1,Nom="Denis Tardiff",NbrArejoindre=1,NbrDesactivee=5,NbrLitiges=2,NbrProspects=6,NbrRDV=8,NbrRejoints=1,NbrRelances=6},
                new NomAppel{Id=2,Nom="Second Example",NbrArejoindre=1,NbrDesactivee=5,NbrLitiges=2,NbrProspects=6,NbrRDV=8,NbrRejoints=1,NbrRelances=6},
                new NomAppel{Id=3,Nom="Third Example",NbrArejoindre=1,NbrDesactivee=5,NbrLitiges=2,NbrProspects=6,NbrRDV=8,NbrRejoints=1,NbrRelances=6}
            };

            return PartialView(nomAppels);
        }

        public ActionResult GetClients()
        {
            List<Client> clients = new List<Client>()
            {
                new Client{ClientId=1,NomClient="ABCD3",FinTerme=new System.DateTime(2010,01,15),Mobile="407 3254 1478",ModeleVehicule="Model vhi 1",Phone1="407 412 145",Phone2="458745 123"},
                new Client{ClientId=2,NomClient="NEWCLIENT",FinTerme=new System.DateTime(2010,01,15),Mobile="407 3254 1478",ModeleVehicule="Model vhi 1",Phone1="407 412 145",Phone2="458745 123"},
                new Client{ClientId=3,NomClient="Example",FinTerme=new System.DateTime(2010,01,15),Mobile="407 3254 1478",ModeleVehicule="Model vhi 1",Phone1="407 412 145",Phone2="458745 123"},
                new Client{ClientId=4,NomClient="Alpha",FinTerme=new System.DateTime(2010,01,15),Mobile="407 3254 1478",ModeleVehicule="Model vhi 1",Phone1="407 412 145",Phone2="458745 123"}
            };

            return PartialView(clients);
        }
        public ActionResult GetFicheClient()
        {
            FicheClient client = new FicheClient()
            {
                Id = 1,
                Courriel = "Sergio@Suly.ca",
                DateAchat = new System.DateTime(2015, 12, 15),
                Langue = "Français",
                Niv = "1450 000 254",
                NoClient = "235 654 741",
                NomClient = "Sergio Palgiaroliui",
                NoteAppelsPrecedents = "alalalalalalalalalllalalalalall",
                Terme = "48 mois",
                ValeurVehicule = "45 0000",
                TypeAchat = "000000",
                VehiculeDesire = "Honda",
                Ville = "Quebec"
            };

            return PartialView(client);
        }

    }
}
