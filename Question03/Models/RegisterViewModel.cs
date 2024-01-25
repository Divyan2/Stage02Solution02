using System.ComponentModel.DataAnnotations;

namespace Question03.Controllers
{
    public class RegisterViewModel
    {
        //name,age,address,username,password,securityquestion and securitycode
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least 8 characters, including uppercase, lowercase, and a number")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Security question is required")]
        public int SecurityQuestionCode { get; set; }

        [Required(ErrorMessage = "Security answer is required")]
        public string SecurityAnswer { get; set; }
    }
}