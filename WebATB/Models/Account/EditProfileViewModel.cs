using System.ComponentModel.DataAnnotations;

namespace WebATB.Models.Account
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Прізвище обов'язкове")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обов'язковий")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Нове фото профілю")]
        public IFormFile? Image { get; set; }

        public string? CurrentImage { get; set; }
    }
}