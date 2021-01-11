using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AnimalShelter.Models.MyValidators
{
    public class PostalCodeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var location = (Location)validationContext.ObjectInstance;
            string postalCode = location.Number;
            bool cond = true;
            if (postalCode[1] - '5' > 0 || postalCode.Length != 6 )
            {
                cond = false;
            }
            
            return cond ? ValidationResult.Success : new ValidationResult("This is not a postal code");
        }

    }
    public class PostalCodeValidator2 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var location = (ShelterLocation)validationContext.ObjectInstance;
            string postalCode = location.Number;
            bool cond = true;
            if (postalCode[1] - '5' > 0 && postalCode.Length != 6)
            {
                cond = false;
            }

            return cond ? ValidationResult.Success : new ValidationResult("This is not a postal code");
        }

    }
}