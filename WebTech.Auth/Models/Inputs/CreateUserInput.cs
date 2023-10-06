namespace WebTech.Auth.Models.Inputs;

public class CreateUserInput
{
    public string Email { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
}