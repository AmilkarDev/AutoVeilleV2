using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models
{
    public class Evenement
    {
        public int IdEvenement { get; set; }
	    public DateTime? DateEvenementDebut {get;set;}
	    public DateTime? DateEvenementFin {get;set;}
	    public int TotalEvenements {get;set;}
	    public DateTime DateCreation  {get;set;}
	    public string Utilisateur  {get;set;}
	    public int NoCommerce  {get;set;}
	    public int AppelsPrevusDirEv  {get;set;}
	    public int AppelsPrevusCASuly  {get;set;}
	    public DateTime? DateModification  {get;set;}
	    public string UtilisateurModification {get;set;}
	    public int DatesConfirmer {get;set;}
    }
}
