using System.ComponentModel.DataAnnotations;

namespace Blog.PL.Areas.User.ViewModels.User
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmed password do not match.")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirmation { get; set; }
    }
}
