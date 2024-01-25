using System.ComponentModel.DataAnnotations;

namespace Question03.Controllers
{
    public class RecoverPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Security code is required")]
        public int SecurityCode { get; set; }

        [Required(ErrorMessage = "Security answer is required")]
        public string SecurityAnswer { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, including uppercase, lowercase, and a number")]
        public string NewPassword { get; set; }
    }
}