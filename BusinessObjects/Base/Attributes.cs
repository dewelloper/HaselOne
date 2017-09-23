using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Base
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredDate : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var d =new DateTime();
                if (DateTime.TryParse(value.ToString(), out d))
                {
                    if (d.ToShortDateString() != new DateTime(0001, 01, 01).ToShortDateString())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    /// <summary>
    /// SELECT LISTLER CLIENT DAN 0 GELIYOR. 0 DAN BUYUK OLMALI VEYA NULL OLMALI NULLABLE REALATION LAR ICIN
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class SelectReqNullAttribute : ValidationAttribute
    {
        public bool AllowEmptyStrings { get; set; }
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            if (Convert.ToInt32(value) > 0)
                return true;

            return false;


        }

        public bool IsValid(int? value)
        {
            if (value == null)
                return true;

            if (value > 1)
                return true;

            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ZeroGreterReqAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            if (Convert.ToInt32(value) > 0)
                return true;

            return false;
        }

        public bool IsValid(int value)
        {

            if (value > 0)
                return true;

            return false;
        }
    }

    public static class ValidationHelper
    {

        public static void Validate(object instance)
        {
            List<ValidationResult> results;
            var isValid = IsValid(instance, out results);

            if (!isValid)
                throw new ValidationException();
        }

        public static bool IsValid(object instance)
        {
            var vc = new ValidationContext(instance, null, null);

            return Validator.TryValidateObject(instance, vc, null, true);
        }

        public static bool IsValid(object instance, out List<ValidationResult> results)
        {
            var vc = new ValidationContext(instance, null, null);
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(instance, vc, results, true);
        }
    }

}
