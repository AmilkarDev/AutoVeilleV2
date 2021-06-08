using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autoveille.TestingModels
{
    public enum Situation {  vrai , faux , nonSpecifiee};

    public class RendezVous
    {
        public int RdvId { get; set; }
        public Situation  Conf { get; set; }
        public Situation WalkIn { get; set; }
        public Situation  Vente { get; set; }

        public string Categorie  { get; set; }
        public string PrenomClient { get; set; }
        public string  NomClient { get; set; }

        public  DateTime DateRdv { get; set; }

        public string  Vendeur { get; set; }

        public string Telephone1 { get; set; }

        public string Telephone2 { get; set; }

        public string Cellulaire { get; set; }

        public string CodeConsultant { get; set; }

        public string  NotesTelInitiales { get; set; }

        public string  NotesTelConfirmation { get; set; }
        public string  NotesConsultant { get; set; }

        public string CourrielClient { get; set; }

    }
}