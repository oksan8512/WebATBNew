namespace WebATB.Areas.Admin.Models.Users;

public class UserItemVM
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = [];
    public string? Image { get; set; }
    public string FullName { get; set; } = string.Empty;
}