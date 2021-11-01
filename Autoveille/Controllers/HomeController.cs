using Autoveille.Models;
using Autoveille.TestingModels;
using AutoveilleBL;
using AutoveilleBL.Models.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Outils;
using Rotativa.MVC;
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

        public ActionResult ExportToPdf()
        {
            var dashboard = new Dashboard()
            {
                RDV = 24,
                Ventes = 15,
                Oportunites = 94,
                Potentiels = 300,
                WalkIn = 32,
            };

            var report = new ViewAsPdf("Dashboard", dashboard);
            return report;
        }

        //public static byte[] PDFForHtml(string html)
        //{
        //    // Create ABCpdf Doc object
        //    var doc = new Doc();

        //    // Add html to Doc
        //    int theID = doc.AddImageHtml(html);

        //    // Loop through document to create multi-page PDF
        //    while (true)
        //    {
        //        if (!doc.Chainable(theID))
        //            break;
        //        doc.Page = doc.AddPage();
        //        theID = doc.AddImageToChain(theID);
        //    }

        //    // Flatten the PDF
        //    for (int i = 1; i <= doc.PageCount; i++)
        //    {
        //        doc.PageNumber = i;
        //        doc.Flatten();
        //    }

        //    // Get PDF as byte array. Couls also use .Save() to save to disk
        //    var pdfbytes = doc.GetData();

        //    doc.Clear();

        //    return pdfbytes;
        //}

        //public static string RenderViewToString(Controller controller, string viewName, object model, string masterName)
        //{
        //    controller.ViewData.Model = model;

        //    using (StringWriter sw = new StringWriter())
        //    {
        //        ViewEngineResult viewResult = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, masterName);
        //        ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
        //        viewResult.View.Render(viewContext, sw);

        //        return sw.GetStringBuilder().ToString();
        //    }
        //}

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

        public ActionResult EvenementEnCours(int? IdConcessionnaire)
        {
            var dashboard = new Dashboard()
            {
                RDV = 24,
                Ventes = 15,
                Oportunites = 94,
                Potentiels = 300,
                WalkIn = 32,
            };

            return PartialView(dashboard);
        }
        // Page 17 du maquette
        public ActionResult ReportEventsDashboard()
        {
            return View();
        
        }

        public ActionResult RapportsEvenementEnCours()
        {
            return View();
        }

        public ActionResult EvenementParFabricant()
        {
            List<ConcessionnaireParFabricant> concessions = new List<ConcessionnaireParFabricant>
            {
                new ConcessionnaireParFabricant
                {
                    Id=0,
                    NomConcessionnaire="Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },
                new ConcessionnaireParFabricant
                {
                    Id=1,
                    NomConcessionnaire="Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },

                new ConcessionnaireParFabricant
                {
                    Id = 2,
                    NomConcessionnaire = "Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },

                new ConcessionnaireParFabricant
                {
                    Id=3,
                    NomConcessionnaire="Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },
                new ConcessionnaireParFabricant
                {
                    Id = 4,
                    NomConcessionnaire = "Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },
            };
            return PartialView(concessions);
        }


        public ActionResult EvenementAvenirParFabricant()
        {
            List<ConcessionnaireParFabricant> concessions = new List<ConcessionnaireParFabricant>
            {
                new ConcessionnaireParFabricant
                {
                    Id=0,
                    NomConcessionnaire="Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },
                new ConcessionnaireParFabricant
                {
                    Id=1,
                    NomConcessionnaire="Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },

                new ConcessionnaireParFabricant
                {
                    Id = 2,
                    NomConcessionnaire = "Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },

                new ConcessionnaireParFabricant
                {
                    Id=3,
                    NomConcessionnaire="Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },
                new ConcessionnaireParFabricant
                {
                    Id = 4,
                    NomConcessionnaire = "Nom Concessionnaire",
                    DateEvenement = "du 00 au 00 mois 2021"
                },
            };
            return PartialView(concessions);
        }
        public ActionResult TableauDeBordDesFabicants()
        {
            return View();
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

        public ActionResult DisplayDashboard()
        {
            return View();
        }

        public ActionResult FutureEventDashboard()
        {
            var dashboard = new Dashboard()
            {
                RDV = 24,
                Ventes = 15,
                Oportunites = 94,
                Potentiels = 300,
                WalkIn = 32,
            };
            return PartialView(dashboard);
        }

        public ActionResult ExampleFutureEventDashboard()
        {
            var dashboard = new Dashboard()
            {
                RDV = 24,
                Ventes = 15,
                Oportunites = 94,
                Potentiels = 300,
                WalkIn = 32,
            };
            return View(dashboard);
        }
        public ActionResult InfoConcession()
        {
            string role ;
            if (Session["User"]!=null)
            {
                role = (string)Session["Role"];
            }
            else
            {
                return RedirectToAction("Connexion", "Compte");
            }
            if (role  == "Gestionnaire")
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


        public ActionResult RendezVous()
        {
            var role = "";
            if (Session["User"] != null)
            {
                role = Utilisateurs.GetRoles(Session["User"].ToString());
            }
            if (role == "Gestionnaire")
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
                    DateDebut = evenement.DateEvenementDebut,
                    DateFin = evenement.DateEvenementFin,
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


        public ActionResult CurrentEventDashboard()
        {
            return View();
        }

        public ActionResult GetRdvClients()
        {
            List<RendezVous> RdvList = new List<RendezVous>
            {
                new RendezVous {RdvId=0,Conf = Situation.faux, WalkIn = Situation.vrai, Vente = Situation.nonSpecifiee, Categorie="Alberto",PrenomClient="marten", NomClient="Colzone", DateRdv = new DateTime(2019,01,24,15,30,00),
                    Vendeur = "Monica",Telephone1="240-569-5827", Telephone2="500-526-3400",Cellulaire="478-256-8545",CodeConsultant="AA",
                    NotesTelInitiales="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    NotesTelConfirmation="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    NotesConsultant="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    CourrielClient="alpha@internet.ca", },


                new RendezVous {RdvId=1,Conf = Situation.faux, WalkIn = Situation.vrai, Vente = Situation.vrai, Categorie="Kaptaa",PrenomClient="jinkez", NomClient="Alex", DateRdv = new DateTime(2021,12,20,12,15,00),
                    Vendeur = "Andry",Telephone1="520-569-5827", Telephone2="477-222-5267",Cellulaire="478-300-8545",CodeConsultant="AA",
                    NotesTelInitiales="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    NotesTelConfirmation="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    NotesConsultant="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    CourrielClient="alpha@internet.ca", },



                new RendezVous {RdvId=2,Conf = Situation.vrai, WalkIn = Situation.faux, Vente = Situation.faux, Categorie="kaptaa", PrenomClient="almore",NomClient="moliere", DateRdv = new DateTime(2020,09,15,13,30,00),
                    Vendeur = "Chaima",Telephone1="360-569-5827", Telephone2="477-526-5267",Cellulaire="478-256-8545",CodeConsultant="AA",
                    NotesTelInitiales="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    NotesTelConfirmation="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    NotesConsultant="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                    CourrielClient="alpha@internet.ca", },



                new RendezVous {RdvId=3,Conf = Situation.nonSpecifiee, WalkIn = Situation.vrai, Vente = Situation.nonSpecifiee, Categorie="Alberto",PrenomClient="bistope", NomClient="Bolto", DateRdv = new DateTime(2021,11,15,13,30,00),
                    Vendeur = "Monica",Telephone1="420-569-5827", Telephone2="477-526-5267",Cellulaire="478-256-8545",CodeConsultant="AA",
                    NotesTelInitiales=" It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                    NotesTelConfirmation="The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.",
                    NotesConsultant="The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.",
                    CourrielClient="alpha@internet.ca", },



                new RendezVous {RdvId=4,Conf = Situation.faux, WalkIn = Situation.vrai, Vente = Situation.vrai, Categorie="kaptaa",PrenomClient="astoli", NomClient="Colzone", DateRdv = new DateTime(2021,10,15,13,30,00),
                    Vendeur = "Monica",Telephone1="200-569-5827", Telephone2="477-526-5267",Cellulaire="478-256-8545",CodeConsultant="AA",
                    NotesTelInitiales=" It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                    NotesTelConfirmation=" It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                    NotesConsultant="The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.",
                    CourrielClient="alpha@internet.ca", },

                new RendezVous {RdvId=5,  Conf = Situation.vrai, WalkIn = Situation.vrai, Vente = Situation.vrai, Categorie="Mirna", PrenomClient="binorta", NomClient="ALba", DateRdv = new DateTime(2021,10,15,13,30,00),
                    Vendeur = "Manir",Telephone1="200-569-5827", Telephone2="477-526-5267",Cellulaire="478-256-8545",CodeConsultant="AA",
                    NotesTelInitiales=" In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best, every pleasure is to be welcomed and every pain avoided. But in certain circumstances and owing to the claims of duty or the obligations of business it will frequently occur that pleasures have to be repudiated and annoyances accepted.",
                    NotesTelConfirmation=" In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best, every pleasure is to be welcomed and every pain avoided. But in certain circumstances and owing to the claims of duty or the obligations of business it will frequently occur that pleasures have to be repudiated and annoyances accepted.",
                    NotesConsultant="The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from \"de Finibus Bonorum et Malorum\" by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.",
                    CourrielClient="alpha@internet.ca", }
            };

            return PartialView(RdvList);
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
               // var noteGeneral = Ventes.GetNotesGeneral(noCommerce, (int)noClient);
                client.NotesAppelsPrecedentes = new List<NoteAppel>();
                client.NotesAppelsPrecedentes = notes;
                //client.NotesGenerals = noteGeneral;
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


        [HttpGet]
        public ActionResult GetRdvDetails(int Idrdv)
        {
            RdvDetails RdvDetail = new RdvDetails
            {
                Annee = "2015",
                ApprobationCredit = "N/A",
                EssaiRoutier = "Non",
                CodeConsultant = "AA",
                VehiculeEchange = "Alpha",
                ListeNoire = "Non",
                Modele = "Ford",
                Offre = "1520",
                Presence = true,
                Vendeur = "Monica",
                VehiculeOccasion = "Beta",
                VegiculeDesire = "Porsche",
                Vente = false,
                NoteCentreAppel = "vfvcvcxvxvcbvcbxcvbcv",
                NoteConsultant = "jhjhgjhgjhggjh"
            };

            return PartialView(RdvDetail);
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


        [HttpGet]
        public ActionResult GetCalendarRendezVous()
        {
            return PartialView();
        }

        public List<Reservation> retourAppelsList = new List<Reservation>
        {
                new Reservation {title="R-A 1", color="red", start = new DateTime(2021,05,22,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22,13,30,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-A 2", color="red", start = new DateTime(2021,05,22,12,00,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-A 3", color="red", start = new DateTime(2021,05,22,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22,13,00,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-A 4", color="red", start = new DateTime(2021,05,22,10,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22,11,00,00).ToString("yyyy-MM-ddTHH:mm:ss")},
            new Reservation {title="R-V 1", allDay = true,color="red", start = new DateTime(2021,05,22).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22).ToString("yyyy-MM-ddTHH:mm:ss")}
        };

        public List<Reservation> rendezVousList = new List<Reservation>
        {
                new Reservation {title="R-V 1", color="green", start = new DateTime(2021,05,22,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22,13,30,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-V 2", color="green", start = new DateTime(2021,05,22,12,00,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-V 3", color="green", start = new DateTime(2021,05,22,12,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22,13,00,00).ToString("yyyy-MM-ddTHH:mm:ss")},
                new Reservation {title="R-V 4", color="green", start = new DateTime(2021,05,22,10,30,00).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22,11,00,00).ToString("yyyy-MM-ddTHH:mm:ss")},
            new Reservation {title="R-V 1", allDay = true,color="green", start = new DateTime(2021,05,22).ToString("yyyy-MM-ddTHH:mm:ss"), end= new DateTime(2021,05,22).ToString("yyyy-MM-ddTHH:mm:ss")}
        };


        [HttpGet]
        public ActionResult GetRendezVousReservations()
        {
            

            return Json(rendezVousList,JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult GetRetourAppelReservations()
        {


            return Json(retourAppelsList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SaveRendezVous(Reservation rendezVous)
        {
            rendezVous.color = "green";
            rendezVousList.Add(rendezVous);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        public ActionResult newWalkIn()
        {
            return PartialView();
        }
    }
}
