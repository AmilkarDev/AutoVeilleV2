using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autoveille.TestingModels
{
    public class RdvDetails
    {
        public bool Presence { get; set; }
        public bool Vente { get; set; }

        public string Vendeur { get; set; }
        public string CodeConsultant { get; set; }

        public string  VegiculeDesire { get; set; }

        public string   Modele { get; set; }

        public string   Annee { get; set; }

        public string EssaiRoutier { get; set; }
        public string ApprobationCredit { get; set; }

        public string  Offre { get; set; }

        public string VehiculeOccasion { get; set; }

        public string ListeNoire { get; set; }

        public string VehiculeEchange { get; set; }

        public string NoteConsultant { get; set; }

        public string NoteCentreAppel { get; set; }
    }
}