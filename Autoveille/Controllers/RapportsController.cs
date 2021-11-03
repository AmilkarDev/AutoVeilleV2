using Autoveille.TestingModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Autoveille.Controllers
{
    public class RapportsController : Controller
    {
        // GET: Rapports
        public ActionResult Index()
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

        public ActionResult RapportsEvenementEnCours()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult allComparateur(List<int> checkedEvents) 
        {
            TempData["checkedEvents"] = checkedEvents;
            return Json("successfully transported check events ids",JsonRequestBehavior.AllowGet);
        }

        public ActionResult Comparateur()
        {
            var checkedEvents = TempData["checkedEvents"] as List<int>;
            if (checkedEvents != null)
            {
                List<Dashboard> dashboards = new List<Dashboard>();
                foreach (int i in checkedEvents)
                {
                    dashboards.Add(new Dashboard { IdEvenement = i, WalkIn = 50, DateEvenement = new DateTime(2020 / 12 / 10), Oportunites = 10, Potentiels = 20, RDV = 15, Ventes = 12 });
                }
                return View(dashboards);
            }
            return View("selectionnerEvents");
        }

        public ActionResult selectionnerEvents()
        {
            return View();
        }

        public ActionResult Rapports_PartialCurrentEvent(int eventId)
        {
            var dashboard = new Dashboard()
            {
                IdEvenement = eventId,
                RDV = 24,
                Ventes = 15,
                Oportunites = 94,
                Potentiels = 300,
                WalkIn = 32,
            };
            return PartialView(dashboard);
        }
    }
}