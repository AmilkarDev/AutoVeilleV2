using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models
{
    public class Relance
    {
        public int Id { get; set; }
        public int NoClient { get; set; }
        public string NoSerie { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Compagnie { get; set; }
        public string Langue { get; set; }
        public string TelephoneResidence { get; set; }
        public string TelephoneTravail { get; set; }
        public string Cellulaire { get; set; }
        public string ExtTravail { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public int Annee { get; set; }
        public string Email { get; set; }
        public string NomSignataire { get; set; }
        public DateTime DateAchat { get; set; }
        public int NbreMois { get; set; }
        public DateTime FinDuTerme { get; set; }
        public string AchatLocation { get; set; }
        public string EtatVehicule { get; set; }
        public int IdEvenement { get; set; }
        public int Resultat { get; set; }
        public int DateResultat { get; set; }
        public string Type { get; set; }
        public string Provenance { get; set; }
        public int IdRelanceCASuly { get; set; }
        public string RaisonDesactivation { get; set; }
        public string NoteSulyService { get; set; }
        public string NoteSulyVente { get; set; }
        public string NoteAppel { get; set; }
        public Decimal ValeurVehiculeCBB { get; set; }
        public DateTime DateCalculeValeurVehiculeCBB { get; set; }
        public DateTime ProchaineRelance { get; set; }
        public DateTime DateRDVService { get; set; }
        public string HeureRDVService { get; set; }
        public DateTime DateRDV { get; set; }
        public string HleureRDV { get; set; }
        public DateTime DateRDVAnt { get; set; }
        public string HeureRDVAnt { get; set; }
        public int RelanceCASuly { get; set; }
        public DateTime DateEquite { get; set; }
        public DateTime Date60Terme { get; set; }
        public int IsCASuly { get; set; }
        public string LeadMarqueDes { get; set; }
        public string LeadModeleDes { get; set; }
        public int LeadAnneeDes { get; set; }
        public int Km { get; set; }
        public DateTime LeadDateRDVDes { get; set; }
        public string LeadHleureRDVDes { get; set; }
        public string LeadNote { get; set; }
        public string LeadAgent { get; set; }
        public DateTime DateProchaineRelance { get; set; } 
    }
}
