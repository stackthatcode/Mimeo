using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mimeo.Blocks.Validation
{
    public class ValidationResults
    {
        public bool IsValid { get; set; }
        public List<ValidationResult> Validation { get; set; }
    }

    public static class ValidationExtensions
    {
        public static ValidationResults Validate(this object input)
        {
            var output = new ValidationResults()
            {
                Validation = new List<ValidationResult>()
            };
            var context = new ValidationContext(input, serviceProvider: null, items: null);
            output.IsValid = Validator.TryValidateObject(input, context, output.Validation, true);
            return output;
        }
    }
}
