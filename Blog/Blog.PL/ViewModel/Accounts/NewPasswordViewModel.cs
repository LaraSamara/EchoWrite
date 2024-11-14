using System.ComponentModel.DataAnnotations;

namespace Blog.PL.ViewModel.Accounts
{
    public class NewPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }   
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmed password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}
