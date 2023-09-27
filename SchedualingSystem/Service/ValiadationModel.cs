using SchedualingSystem.Models.DTO.Request;
using System.ComponentModel.DataAnnotations;

namespace SchedualingSystem.Service
{
    public static class ValiadationModel
    {
        public static void ValidateModel(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool validate = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!validate)
            {
                throw new ArgumentException(validationResults.FirstOrDefault().ErrorMessage);
            }
        }
    }
}
