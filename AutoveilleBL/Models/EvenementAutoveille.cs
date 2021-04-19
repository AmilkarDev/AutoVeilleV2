using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models
{
    public class EvenementAutoveille
    {
        public int IdEvenement { get; set; }

        public DateTime DateEvenementDebut { get; set; }
        public DateTime DateEvenementFin { get; set; }
        public int AppelsPrevusDirEv { get; set; }
        public int AppelsPrevusCASuly { get; set; }
        public int NoCommerce { get; set; }
        public int NbreAppelsEquite { get; set; }
        public int NbreAppelsLocationEquite { get; set; }
        public int NbreAppelsLocation { get; set; }
        public int NbreAppelsFinancementTaktic { get; set; }
        public int NbreAppelsServiceTaktic { get; set; }
        public int NbreAppelsWalkOut { get; set; }
        public int NbreAppelsLeads { get; set; }
        public int NbreAppelsRDVService { get; set; }
        public int NbreAppelsFinancementTakticLandingPage { get; set; }
        public int NbreAppelsServiceTakticLandingPage { get; set; }

    }
}
