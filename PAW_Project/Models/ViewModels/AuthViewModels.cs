using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PAW_Project.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Parola trebuie să aibă minim {2} caractere.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă Parola")]
        [Compare("Password", ErrorMessage = "Parolele nu se potrivesc.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Poza de profil este obligatorie.")]
        public IFormFile ProfilePicture { get; set; }
    }
}