using System.ComponentModel.DataAnnotations;

namespace LogCentralPlatform.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}