using Autoveille.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autoveille.TestingModels;
using System.ComponentModel.DataAnnotations;
using AutoveilleBL.Models;

namespace Autoveille.TestingModels
{
    public class Client
    {
        public int IdFiche { get; set; }
        [Display(Name = "Client no")]
        public int ClientId { get; set; }
        public string NomClient { get; set; }
        public string  PrenomClient { get; set; }
        public string  Compagnie { get; set; }
        public string ModeleVehicule { get; set; }
        public DateTime FinTerme { get; set; }
        public string FinTermeText { get; set; }
        [Display(Name = "Téléphone residence")]
        public string Phone1 { get; set; }
        [Display(Name = "Téléphone travail")]
        public string Phone2 { get; set; }
        [Display(Name = "Cellulaire")]
        public string Mobile { get; set; }
        public string Langue { get; set; }
        public string Courriel { get; set; }
        public string Adresse { get; set; }
        public string Ville { get; set; }
        public int NumClient { get; set; }
        public string ModifClient { get; set; }
        public string NoteAppel { get; set; }
        public List<NoteAppel> NotesAppelsPrecedentes { get; set; }
        public string NotesGenerals { get; set; }

        public Vehicule Vehicule { get; set; }

        [Display(Name = "Client/opportunité")]
        public string NomComplet 
        {
            get
            {
                var res = String.IsNullOrEmpty(PrenomClient) ? "" : PrenomClient;
                res = res+ " " + (String.IsNullOrEmpty(NomClient) ? "" : NomClient);
                res = res.Trim();
                res = String.IsNullOrEmpty(res) ? (String.IsNullOrEmpty(Compagnie) ? "" : Compagnie) : res;
                return res;
            }
        }
    }
}