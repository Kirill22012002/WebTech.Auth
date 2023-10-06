namespace WebTech.Auth.Models.Inputs;

public class ChangeUserInfoInput
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
}