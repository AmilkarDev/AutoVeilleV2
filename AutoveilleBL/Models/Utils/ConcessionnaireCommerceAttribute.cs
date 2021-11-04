using AutoveilleBL.Models.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoveilleBL.Models.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConcessionnaireCommerceAttribute : ValidationAttribute
    {
        private string RoleFieldName { get; set; }
        private string displayName;
        public ConcessionnaireCommerceAttribute(string roleFieldName)
        {
            RoleFieldName = roleFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(string.Format("{0} est obligatoire!", "L'id Concessionnaire"));
            }
            int commerceId = (int)value;
            var userRole = validationContext.ObjectType.GetProperty(RoleFieldName).GetValue(validationContext.ObjectInstance, null);
            displayName = validationContext.DisplayName;

            if ((commerceId > 0 && Roles.Concessionnaire.Equals((Roles)userRole)) || (!Roles.Concessionnaire.Equals((Roles)userRole)))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Si le role est \"Concessionnaire\" le commerce doit être selectionné !");
            }
        }
    }
}
