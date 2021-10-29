using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Autoveille.Utils;

namespace AutoveilleBL.Models
{
    public class Evenement
    {
        public int IdEvenement { get; set; }

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
		[DisplayName("date début evenement")]
		[Display(Name = "Date Début èvénement")]
		public DateTime? DateEvenementDebut {get;set;}

		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
		[DateGreaterThan("DateEvenementDebut")]
		[Display(Name = "Date Fin évènement")]
		[DisplayName("Date Fin évènement")]
		public DateTime? DateEvenementFin {get;set;}
	    public int TotalEvenements {get;set;}
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString ="{0:dd-MM-yyy}",ApplyFormatInEditMode =true)]
        [Required(ErrorMessage = "Date creation est obligatoire")]
		public DateTime DateCreation  {get;set;}
		[Required(ErrorMessage ="Nom d'utilisateur est obligatoire")]
	    public string Utilisateur  {get;set;}
		[Required(ErrorMessage ="No du commerce est obligatoire")]
	    public int NoCommerce  {get;set;}
	    public int AppelsPrevusDirEv  {get;set;}
	    public int AppelsPrevusCASuly  {get;set;}
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd-MM-yyy}", ApplyFormatInEditMode = true)]
		public DateTime? DateModification  {get;set;}
	    public string UtilisateurModification {get;set;}
	    public int DatesConfirmer {get;set;}
		[Required]
		public bool Actif { get; set; }
		public List<ListeAppels> ListeAppels { get; set; }
    }
}
