using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Autoveille.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateGreaterThanAttribute: ValidationAttribute
    {
        private string DateToCompareFieldName { get; set; }
        private string displayName;
        public DateGreaterThanAttribute(string dateToCompareFieldName)
        {
            DateToCompareFieldName = dateToCompareFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
             DateTime laterDate = (DateTime)value;

            DateTime earlierDate = (DateTime)validationContext.ObjectType.GetProperty(DateToCompareFieldName).GetValue(validationContext.ObjectInstance, null);
            displayName = validationContext.DisplayName;
            if (laterDate > earlierDate)
            {
                return ValidationResult.Success;
            }
            else
            {               
                return new ValidationResult(string.Format("{0} précisé est pas valide considérant la date début!", displayName));
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = string.Format("{0} précisé est pas valide considérant la date début!", displayName  ),
                ValidationType = "dategreaterthan"
            };

            clientValidationRule.ValidationParameters.Add("datetocomparefieldname", DateToCompareFieldName);

            return new[] { clientValidationRule };
        }

    }
}
