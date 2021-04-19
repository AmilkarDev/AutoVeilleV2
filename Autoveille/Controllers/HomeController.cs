using Autoveille.Models;
using Autoveille.TestingModels;
using AutoveilleBL;
using AutoveilleBL.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult InfoConcession()
        {
            var role = 0;
            if (Session["User"]!=null)
            {
                role = Utilisateurs.GetRoles(Session["User"].ToString());
            }
            if (role==1)
            {
                int userId = -1;
                bool resParse;

                //to do : a verifier role : consultant

                resParse = int.TryParse(Session["UserId"].ToString(), out userId);
                //to do
                if (!resParse)
                {
                    //return "error user id pas bon";
                }
                var evenements = Ventes.GetEvenementsByConsultant(userId);
                if (evenements == null || evenements.Count == 0)
                {
                    //return "aucune evenement n'est pas associé à l'utilisateur";
                }
                //to do
                if (evenements.Count >= 2)
                {
                    //return "il y a plus d'un evenement associé à l'utilisateur";
                }
                var evenement = evenements.FirstOrDefault();
                evenement = Ventes.GetEvenement(evenement.IdEvenement, evenement.NoCommerce); ;
                var nomCommerce = Concessions.GetNomCommerce(evenement.NoCommerce);
                var evenementUI = new EvenementModel()
                {
                    NoCommerce = evenement.NoCommerce,
                    NomCommerce = nomCommerce,
                    DateDebut=evenement.DateEvenementDebut,
                    DateFin=evenement.DateEvenementFin,
                };
                return View(evenementUI);
            }
            //if (Session["User"] != null )
            //{
            //    var role= Utilisateurs.GetRoles(Session["User"].ToString(), aNoCommerce);
            //    if (role != null)
            //    {
            //        Session["NoCommerce"] = aNoCommerce;
            //        Session["Role"] = role.Role;
            //    }
            //    else
            //    {
            //        return RedirectToAction("Connexion", "Compte");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Connexion", "Compte");
            //}
            //var ventes = Ventes.GetVentes(aNoCommerce);
            ////var appels = new Appels();
            ////appels.Evenement = Ventes.GetProchaineEvenement(aNoCommerce);
            ////appels.Relances = Ventes.GetRelances(appels.Evenement.IdEvenement, aNoCommerce);
            return View();
        }



        //Data testing methods 

        public ActionResult GetNomAppels()
        {
            int userId=-1;
            bool resParse;

            //to do : a verifier role : consultant

            resParse = int.TryParse(Session["UserId"].ToString(), out userId);
            //to do
            if (!resParse)
            {
                //return "error user id pas bon";
            }

            var evenements = Ventes.GetEvenementsByConsultant(userId);
            //to do
            if (evenements == null || evenements.Count==0)
            {
                //return "aucune evenement n'est pas associé à l'utilisateur";
            }
            //to do
            if (evenements.Count >= 2)
            {
                //return "il y a plus d'un evenement associé à l'utilisateur";
            }
            List<NomAppel> nomAppels = new List<NomAppel>();
            //List<NomAppel> nomAppels = new List<NomAppel>
            //{
            //    new NomAppel{Id=1,Nom="Denis Tardiff",NbrArejoindre=1,NbrDesactivee=5,NbrLitiges=2,NbrProspects=6,NbrRDV=8,NbrRejoints=1,NbrRelances=6},
            //    new NomAppel{Id=2,Nom="Second Example",NbrArejoindre=1,NbrDesactivee=5,NbrLitiges=2,NbrProspects=6,NbrRDV=8,NbrRejoints=1,NbrRelances=6},
            //    new NomAppel{Id=3,Nom="Third Example",NbrArejoindre=1,NbrDesactivee=5,NbrLitiges=2,NbrProspects=6,NbrRDV=8,NbrRejoints=1,NbrRelances=6}
            //};

            var evenement = evenements.FirstOrDefault();
            var listesAppels = Ventes.GetAppelsConsultantById(evenement.IdEvenement, evenement.NoCommerce);
            Session["IdEvenement"] = evenement.IdEvenement;
            Session["NoCommerce"] = evenement.NoCommerce;
            foreach(var liste in listesAppels.OrderBy(o=>o.OrdreAfichagePortail))
            {
                var nomAppel = new NomAppel()
                {
                    Id= liste.TypeAppel,
                    Nom=liste.DescriptionListe,
                    NbrProspects=liste.NbreAppels
                };
                nomAppels.Add(nomAppel);
            }
            return PartialView(nomAppels);
        }

        public ActionResult GetClients(int aIdTypeEvenement)
        {
            var idEvenement = int.Parse(Session["IdEvenement"].ToString());
            var noCommerce = int.Parse(Session["NoCommerce"].ToString());
            var relances = Ventes.GetRelances(noCommerce, idEvenement, aIdTypeEvenement);
            List<Client> clients = new List<Client>();
            foreach (var r in relances)
            {
                var client = new Client();
                client.ClientId = r.Id;
                client.NomClient = ((r.Prenom ?? "") + " " + (r.Nom ?? "") + " " + (r.Compagnie ?? "")).Trim();
                client.ModeleVehicule = (r.Modele + " " + (r.Annee==null?"":r.Annee.ToString())).Trim();
                client.Phone1 = r.TelephoneResidence??"";
                client.Phone2 = (r.TelephoneTravail ?? "" + " " + r.ExtTravail ?? "").Trim();
                client.Mobile = r.Cellulaire ?? "";
                client.FinTermeText = r.FinDuTerme==null?"":((DateTime) r.FinDuTerme).ToString("dd-mm-yyyy");
                clients.Add(client);
            }
            //List<Client> clients = new List<Client>()
            //{
            //    new Client{ClientId=1,NomClient="ABCD3",FinTerme=new System.DateTime(2010,01,15),Mobile="407 3254 1478",ModeleVehicule="Model vhi 1",Phone1="407 412 145",Phone2="458745 123"},
            //    new Client{ClientId=2,NomClient="NEWCLIENT",FinTerme=new System.DateTime(2010,01,15),Mobile="407 3254 1478",ModeleVehicule="Model vhi 1",Phone1="407 412 145",Phone2="458745 123"},
            //    new Client{ClientId=3,NomClient="Example",FinTerme=new System.DateTime(2010,01,15),Mobile="407 3254 1478",ModeleVehicule="Model vhi 1",Phone1="407 412 145",Phone2="458745 123"},
            //    new Client{ClientId=4,NomClient="Alpha",FinTerme=new System.DateTime(2010,01,15),Mobile="407 3254 1478",ModeleVehicule="Model vhi 1",Phone1="407 412 145",Phone2="458745 123"}
            //};

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


        public ActionResult GetMainClient()
        {
            Vehicule vehicule = new Vehicule()
            {
                Annee = "2019",
                DateAcquisition = new System.DateTime(2015, 12, 15),
                TypeAchat = "Financement",
                EtatVente = "Neuf",
                FinTerme = new System.DateTime(2015, 12, 15),
                Marque = "Genesis",
                Modele = "Genesis G70",
                Niv = "alalalalalalalalalllalalalalall",
                Terme = "48 mois",
                ValeurVehicule = "45 0000",
                VehiculeDesire = "Honda",
                ValeurActuel = "25 000"

            };

            Client client = new Client()
            {
                ClientId = 1,
                NomClient = "sergio makrov",
                Adresse = "Lyon 2501 rue des elites",
                Compagnie = "MaCompagnie",
                Courriel = "Sergio.Makrov",
                FinTerme = new System.DateTime(2012, 10, 15),
                Langue = "Anglais",
                Mobile = "1407 32814 5647",
                ModeleVehicule= "Genesis G70",
                NumClient= 45872 ,
                Phone1="4078562 1547",
                Phone2= "741 2589 6325",
                PrenomClient="vebrfol",
                Ville="Lyon",
                ModifClient = "Aucune modification n'a été apporté à ce client"
            };

            return PartialView(client);
        }


        public ActionResult GetVehicule()
        {
            Vehicule vehicule = new Vehicule()
            {
                Annee = "2019",
                DateAcquisition = new System.DateTime(2015, 12, 15),
                TypeAchat = "Financement",
                EtatVente = "Neuf",
                FinTerme = new System.DateTime(2015, 12, 15),
                Marque = "Genesis",
                Modele = "Genesis G70",
                Niv = "alalalalalalalalalllalalalalall",
                Terme = "48 mois",
                ValeurVehicule = "45 0000",
                VehiculeDesire = "Honda",
                ValeurActuel = "25 000"

            };

            return PartialView(vehicule);
        }

    }
}
