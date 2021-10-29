using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autoveille.Utils;

namespace Autoveille.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateGreaterThanAttribute: ValidationAttribute, IClientValidatable
    {
        private string DateToCompareFieldName { get; set; }

        public DateGreaterThanAttribute(string dateToCompareFieldName)
        {
            DateToCompareFieldName = dateToCompareFieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime laterDate = (DateTime)value;

            DateTime earlierDate = (DateTime)validationContext.ObjectType.GetProperty(DateToCompareFieldName).GetValue(validationContext.ObjectInstance, null);

            if (laterDate > earlierDate)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(string.Format("{0} précisé est pas valide considérant la date début!", DateToCompareFieldName));
            }
        }

        //esse método retorna as validações que serão utilizadas no lado cliente
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = string.Format("{0} précisé est pas valide considérant la date début!", DateToCompareFieldName),
                ValidationType = "dategreaterthan"
            };

            clientValidationRule.ValidationParameters.Add("datetocomparefieldname", DateToCompareFieldName);

            return new[] { clientValidationRule };
        }

    }
}
