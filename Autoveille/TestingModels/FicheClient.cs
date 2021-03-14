using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Autoveille.TestingModels
{
    public class FicheClient
    {
        public int Id { get; set; }
        public string NomClient { get; set; }
        public string Langue { get; set; }
        public string Courriel { get; set; }
        public string Ville { get; set; }
        public string NoClient { get; set; }
        public string Niv { get; set; }
        public string TypeAchat { get; set; }
        public DateTime DateAchat { get; set; }
        public string Terme { get; set; }
        public string ValeurVehicule { get; set; }
        public string VehiculeDesire { get; set; }
        public string NoteAppelsPrecedents { get; set; }
    }
}