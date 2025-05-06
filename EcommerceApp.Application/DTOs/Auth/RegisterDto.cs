using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EcommerceApp.Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [CustomPasswordValidation]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string? ReturnUrl { get; set; }
    }

    public class CustomPasswordValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Password is required.");
            }

            // Check for minimum length
            if (password.Length < 6)
            {
                return new ValidationResult("Password must be at least 6 characters long.");
            }

            // Check for at least one uppercase letter
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }

            // Check for at least one lowercase letter
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return new ValidationResult("Password must contain at least one lowercase letter.");
            }

            // Check for at least one digit
            if (!Regex.IsMatch(password, @"\d"))
            {
                return new ValidationResult("Password must contain at least one digit.");
            }

            // Check for at least one non-alphanumeric character
            if (!Regex.IsMatch(password, @"\W"))
            {
                return new ValidationResult("Password must contain at least one non-alphanumeric character.");
            }

            return ValidationResult.Success;
        }
    }
}
