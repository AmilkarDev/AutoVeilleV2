using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models.Web
{
    public enum UserTypes { Suly, Concessionnaire };
    public enum Roles { Gestionnaire, Consultant, Concessionnaire }
    public enum Langues { Anglais, Français }
    public class UtilisateurSite
    {

        public int UserID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select Commerce")]
        public int NoCommerce { get; set; }

        public string NomCommerce { get; set; }

        [Required(ErrorMessage = "Username est obligatoire")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "Huit caractères au moins, au moins une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécia")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire")]
        [CompareAttribute(nameof(Password), ErrorMessage = "Les deux mots de passes doivent être identiques")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        public string LastName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "l'adresse email est obligatoire")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La langue est obligatoire")]
        public Langues Langue { get; set; }

        [Required(ErrorMessage = "Le Role est obligatoire")]
        public Roles Role { get; set; }

        [Required(ErrorMessage = "Le type usager est obligatoire")]
        public UserTypes TypeUsager { get; set; }
    }
}
