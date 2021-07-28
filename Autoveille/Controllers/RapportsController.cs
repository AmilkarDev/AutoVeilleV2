using Autoveille.TestingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}