using System;

namespace Autoveille.Models
{
    public class Vehicule
    {
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Annee { get; set; }
        public string ValeurVehicule { get; set; }
        public string ValeurActuel { get; set; }
        public string Niv  { get; set; }

        public string EtatVente { get; set; }
        public string TypeAchat { get; set; }
        public string Terme { get; set; }
        public DateTime DateAcquisition { get; set; }
        public DateTime FinTerme { get; set; }
        public string VehiculeDesire { get; set;  }
        public string ModifVehicule { get; set; }
    }
}