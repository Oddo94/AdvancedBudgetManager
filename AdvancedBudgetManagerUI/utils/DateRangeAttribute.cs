using System;
using System.ComponentModel.DataAnnotations;

namespace AdvancedBudgetManager.utils {
    public class DateRangeAttribute : ValidationAttribute {
        public string StartProperty;
        public string EndProperty;

        public DateRangeAttribute(string startProperty, string endProperty) {
            StartProperty = startProperty;
            EndProperty = endProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context) {
            Type type = context.ObjectType;
            DateTimeOffset? startValue = type.GetProperty(StartProperty)?.GetValue(context.ObjectInstance) as DateTimeOffset?;
            DateTimeOffset? endValue = type.GetProperty(EndProperty)?.GetValue(context.ObjectInstance) as DateTimeOffset?;

            if (startValue != null && endValue != null && startValue > endValue) {
                return new ValidationResult("The start date must be before end date");
            }

            return ValidationResult.Success;
        }
    }
}
