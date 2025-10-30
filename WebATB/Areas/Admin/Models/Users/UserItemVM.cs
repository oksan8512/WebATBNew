using System.ComponentModel.DataAnnotations;

namespace WebATB.Areas.Admin.Models.Users;

public class UserItemVM
{
    public int Id { get; set; }

    [Display(Name = "Ім'я")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Прізвище")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Фото")]
    public string? Image { get; set; }

    [Display(Name = "Email підтверджено")]
    public bool EmailConfirmed { get; set; }

    [Display(Name = "Дата реєстрації")]
    public DateTime RegistrationDate { get; set; }

    [Display(Name = "Заблокований")]
    public bool IsLocked { get; set; }

    [Display(Name = "Ролі")]
    public List<string> Roles { get; set; } = new();

    public string FullName => $"{FirstName} {LastName}";
}