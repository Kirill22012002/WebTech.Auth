namespace WebTech.Auth.Models.ViewModels;

public class UserViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public IEnumerable<string> Roles { get; set; }
}