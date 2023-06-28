using System.ComponentModel.DataAnnotations;

namespace NewsWebsiteFront.Models
{
    public class DateValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime? date = value as DateTime?;
            if (date == null)
            {
                return false;
            }
            if (date >= DateTime.Today && date <= DateTime.Today.AddDays(7))
            {
                return true;
            }
            return false;
        }
        public override string FormatErrorMessage(string name)
        {
            return "The " + name + " field must be between today and a week from today.";
        }
    }
}
