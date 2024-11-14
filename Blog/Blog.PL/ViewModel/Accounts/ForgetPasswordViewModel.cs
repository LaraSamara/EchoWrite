using System.ComponentModel.DataAnnotations;

namespace Blog.PL.ViewModel.Accounts
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
