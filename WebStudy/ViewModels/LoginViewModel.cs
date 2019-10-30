using System.ComponentModel.DataAnnotations;

namespace WebStudy.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UsernameRequired")]
        [Display(Name = "Username")]
        [EmailAddress]
        public string Username { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [Display(Name = "Password")]
        [MinLength(6)]
        public string Password { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
