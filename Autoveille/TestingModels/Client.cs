using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autoveille.TestingModels
{
    public class Client
    {
        public int IdFiche { get; set; }
        public int ClientId { get; set; }
        public string NomClient { get; set; }
        public string  PrenomClient { get; set; }
        public string  Compagnie { get; set; }
        public string ModeleVehicule { get; set; }
        public DateTime FinTerme { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Mobile { get; set; }
        public string Langue { get; set; }
        public string Courriel { get; set; }
        public string Adresse { get; set; }
        public string Ville { get; set; }
        public int NumClient { get; set; }
        public string ModifClient { get; set; }
    }
}