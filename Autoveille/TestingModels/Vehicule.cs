using System;
using System.ComponentModel.DataAnnotations;

namespace Autoveille.Models
{
    public class Vehicule
    {
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Annee { get; set; }
        public string ValeurVehicule { get; set; }
        public string ValeurActuel { get; set; }
        public string SoldAPayer { get; set; }
        public string Niv  { get; set; }
        public string EtatVente { get; set; }
        public string TypeAchat { get; set; }
        public string Terme { get; set; }
        [Display(Name = "Date d'achat")]
        public string DateAcquisition { get; set; }
        [Display(Name = "Fin du terme")]
        public string FinTerme { get; set; }
        public string VehiculeDesire { get; set;  }
        public string ModifVehicule { get; set; }
        [Display(Name = "Type de vente")]
        public string TypeDeVente
        {
            get
            {
                var res = String.IsNullOrEmpty(TypeAchat) ? "" : TypeAchat;
                switch (res)
                {
                    case "A":
                        // code block
                        res = (String.IsNullOrEmpty(Terme) ? 0:int.Parse(Terme)) == 0 ?
                            "Argent comptant":"Financement";
                        break;
                    case "R":
                        res = (String.IsNullOrEmpty(Terme) ? 0 : int.Parse(Terme)) == 0 ?
                                "Argent comptant" : "Financement";
                        // code block
                        break;
                    case "L":
                        // code block
                        res = "Location";
                        break;
                    default:
                        // code block
                        res = "Inconnu";
                        break;
                }
                res = res.Trim();
                return res;
            }
        }
        [Display(Name = "Véhicule actuel")]
        public string VehiculeActuel
        {
            get
            {
                var res = String.IsNullOrEmpty(Marque) ? "" : Marque;
                res = res + " " + (String.IsNullOrEmpty(Modele) ? "" : Modele);
                res = res + " " + (String.IsNullOrEmpty(Annee) ? "" : Annee);
                res = res.Trim();
                return res;
            }
        }
    }
}