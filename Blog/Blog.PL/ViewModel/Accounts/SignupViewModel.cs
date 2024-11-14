using System.ComponentModel.DataAnnotations;

namespace Blog.PL.ViewModel.Accounts
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "First Name is Required")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Only Character is Valid")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Only Character is Valid")]
        public string LastName { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Address is Required")]
        [RegularExpression(@"^[A-Za-z0-9\s,.'\-\/#]+$", ErrorMessage = "Only Character is Valid")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
