using System;
using System.Globalization;
using System.Windows.Controls;

namespace FuzzyExpert.WpfClient.Validations
{
    public class ConfidenceFactorValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var stringRepresentation = (string)value;
                if (!double.TryParse(stringRepresentation, NumberStyles.Any, cultureInfo, out double confidenceFactor))
                {
                    return new ValidationResult(false, "Invalid symbols");
                }

                if (confidenceFactor < 0 || confidenceFactor > 1)
                {
                    return new ValidationResult(false, "Confidence factor should be in [0;1] range.");
                }

                return ValidationResult.ValidResult;

            }
            catch (Exception exception)
            {
                return new ValidationResult(false, $"Invalid symbols or {exception.Message}");
            }
        }
    }
}