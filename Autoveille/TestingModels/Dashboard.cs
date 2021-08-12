using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autoveille.TestingModels
{
    public class Dashboard
    {
        public int IdEvenement { get; set; }
        public DateTime DateEvenement { get; set; }
        public int RDV { get; set; }
        public int WalkIn { get; set; }
        public int Potentiels { get; set; }
        public int Oportunites { get; set; }
        public int Ventes { get; set; }
    }
}