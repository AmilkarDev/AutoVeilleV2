using Autoveille.Models;
using Autoveille.TestingModels;
using AutoveilleBL;
using AutoveilleBL.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Outils;
using AutoveilleBL.Models;

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
            var dashboard = new Dashboard()
            {
                RDV = 24,
                Ventes = 15,
                Oportunites = 94,
                Potentiels =300,
                WalkIn=32,
            };

            return View(dashboard);
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
                evenement = Ventes.GetEvenement(evenement.IdEvenement, evenement.NoCommerce); 
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
            if (Session["Commerces"] == null || Session["IdEvenement"]==null)
            {
                return RedirectToAction("Connexion", "Compte");
            }
            var idEvenement = int.Parse(Session["IdEvenement"].ToString());
            var noCommerce = int.Parse(Session["NoCommerce"].ToString());
            var relances = Ventes.GetRelances(noCommerce, idEvenement, aIdTypeEvenement);
            List<Client> clients = new List<Client>();
            foreach (var r in relances)
            {
                var client = new Client();
                client.Vehicule = new Vehicule();
                client.ClientId = r.Id;
                client.NomClient = ((r.Prenom ?? "") + " " + (r.Nom ?? "") + " " + (r.Compagnie ?? "")).Trim();
                client.ModeleVehicule = (r.Modele + " " + (r.Annee==null?"":r.Annee.ToString())).Trim();
                client.Phone1 = PhoneNumberUtils.FormatPhoneNumber( r.TelephoneResidence??"");
                client.Phone2 = (PhoneNumberUtils.FormatPhoneNumber(r.TelephoneTravail ?? "" )+ " " + r.ExtTravail ?? "").Trim();
                client.Mobile = PhoneNumberUtils.FormatPhoneNumber(r.Cellulaire ?? "");
                //client.FinTermeText = r.FinDuTerme==null?"":((DateTime) r.FinDuTerme).ToString("dd-mm-yyyy");
                client.Vehicule.TypeAchat=r.AchatLocation ?? "";
                client.Vehicule.Terme = (r.NbreMois == null ? 0 : ((int)r.NbreMois)).ToString();
                client.Vehicule.Marque = r.Marque ?? "";
                client.Vehicule.Modele = r.Modele ?? "";
                client.Vehicule.Annee = (r.Annee==null || r.Annee==0) ? "":r.Annee.ToString();
                client.Vehicule.FinTerme= r.FinDuTerme == null ? "" : ((DateTime)r.FinDuTerme).ToString("dd-MM-yyyy");
                client.Vehicule.DateAcquisition= r.DateAchat == null ? "" : ((DateTime)r.DateAchat).ToString("dd-MM-yyy"); ;

                //var noClient = r.NoClient;
                //if (noClient!=null && noClient>0)
                //{
                //    var notes = Ventes.GetNotesPrecedente(noCommerce, r.Id,(int) noClient);
                //    var noteGeneral = Ventes.GetNotesGeneral(noCommerce,(int) noClient);
                //    client.NotesAppelsPrecedentes = new List<NoteAppel>();
                //    client.NotesAppelsPrecedentes = notes;
                //    client.NotesGenerals = noteGeneral;
                //}
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


        public ActionResult GetMainClient(int aIdFiche)
        {
            var idEvenement = int.Parse(Session["IdEvenement"].ToString());
            var noCommerce = int.Parse(Session["NoCommerce"].ToString());
            var r = Ventes.GetRelance(noCommerce, idEvenement, aIdFiche);
            List<Client> clients = new List<Client>();

        

            Client client = new Client()
            {
                ClientId = aIdFiche,
                NomClient = r.Nom ?? "",
                Adresse = r.Adresse,
                Compagnie = r.Compagnie ?? "",
                Courriel = r.Email,
                //FinTermeText = r.FinDuTerme == null ? "" : ((DateTime)r.FinDuTerme).ToString("dd-mm-yyyy"),
                Langue = r.Langue == "A" ? "Anglais" : "Français",
                Mobile = PhoneNumberUtils.FormatPhoneNumber(r.Cellulaire??""),
                ModeleVehicule = r.Modele,
                NumClient = r.NoClient??0,
                Phone1 = PhoneNumberUtils.FormatPhoneNumber(r.TelephoneResidence??""),
                Phone2 = PhoneNumberUtils.FormatPhoneNumber(r.TelephoneTravail??""),
                PrenomClient = r.Prenom ?? "",
                Ville = r.Ville,

                ModifClient = "Aucune modification n'a été apporté à ce client"
            };
            client.Vehicule = new Vehicule();

            client.Vehicule.Niv = r.NoSerie;
            client.Vehicule.Marque = r.Marque;
            client.Vehicule.Modele = r.Modele;
            client.Vehicule.Annee = r.Annee==null?"":(r.Annee.ToString());
            client.Vehicule.EtatVente = r.EtatVehicule;
            client.Vehicule.DateAcquisition = r.DateAchat==null?"":((DateTime) r.DateAchat).ToString("dd-MM-yyy");
            client.Vehicule.FinTerme = r.FinDuTerme == null ? "" : ((DateTime)r.FinDuTerme).ToString("dd-MM-yyy");

            var noClient = r.NoClient;
            if (noClient != null && noClient > 0)
            {
                var notes = Ventes.GetNotesPrecedente(noCommerce, r.Id, (int)noClient);
                var noteGeneral = Ventes.GetNotesGeneral(noCommerce, (int)noClient);
                client.NotesAppelsPrecedentes = new List<NoteAppel>();
                client.NotesAppelsPrecedentes = notes;
                client.NotesGenerals = noteGeneral;
            }

            //Vehicule vehicule = new Vehicule()
            //{
            //    Annee = "2019",
            //    DateAcquisition = new System.DateTime(2015, 12, 15),
            //    TypeAchat = "Financement",
            //    EtatVente = "Neuf",
            //    FinTerme = new System.DateTime(2015, 12, 15),
            //    Marque = "Genesis",
            //    Modele = "Genesis G70",
            //    Niv = "alalalalalalalalalllalalalalall",
            //    Terme = "48 mois",
            //    ValeurVehicule = "45 0000",
            //    VehiculeDesire = "Honda",
            //    ValeurActuel = "25 000"

            //};

            //Client client = new Client()
            //{
            //    ClientId = 1,
            //    NomClient = "sergio makrov",
            //    Adresse = "Lyon 2501 rue des elites",
            //    Compagnie = "MaCompagnie",
            //    Courriel = "Sergio.Makrov",
            //    FinTerme = new System.DateTime(2012, 10, 15),
            //    Langue = "Anglais",
            //    Mobile = "1407 32814 5647",
            //    ModeleVehicule = "Genesis G70",
            //    NumClient = 45872,
            //    Phone1 = "4078562 1547",
            //    Phone2 = "741 2589 6325",
            //    PrenomClient = "vebrfol",
            //    Ville = "Lyon",
            //    ModifClient = "Aucune modification n'a été apporté à ce client"
            //};

            return PartialView(client);
        }


        public ActionResult GetVehicule()
        {
            Vehicule vehicule = new Vehicule()
            {
                Annee = "2019",
                DateAcquisition = "15-12-2015",
                TypeAchat = "Financement",
                EtatVente = "Neuf",
                FinTerme = "15-12-2015",
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


        [HttpGet]
        public ActionResult EditMainClient()
        {
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
                ModeleVehicule = "Genesis G70",
                NumClient = 45872,
                Phone1 = "4078562 1547",
                Phone2 = "741 2589 6325",
                PrenomClient = "vebrfol",
                Ville = "Lyon",
                ModifClient = "Aucune modification n'a été apporté à ce client"
            };

            return PartialView(client);
        }
    
        [HttpGet]
        public ActionResult GetCalendar()
        {
            return PartialView();
        }


        public List<Reservation> eventsList = new List<Reservation>
        {
                new Reservation {title="R-V 1", color="green", start = new DateTime(2021,05,21,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,15,12,59,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-V 2", color="green", start = new DateTime(2021,05,21,12,00,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,15,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-V 3", color="green", start = new DateTime(2021,05,21,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,15,13,00,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-V 4", color="green", start = new DateTime(2021,05,21,10,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,15,11,00,00).ToString("yyyy-MM-ddTHH:mm:ss")},
            new Reservation {title="R-V 1", allDay = true,color="blue", start = new DateTime(2021,05,15).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,15).ToString("yyyy-MM-ddTHH:mm:ss")}
        };


        [HttpGet]
        public ActionResult GetReservations()
        {
            

            return Json(eventsList,JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult SaveRendezVous(Reservation rendezVous)
        {
            rendezVous.color = "green";
            eventsList.Add(rendezVous);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}
