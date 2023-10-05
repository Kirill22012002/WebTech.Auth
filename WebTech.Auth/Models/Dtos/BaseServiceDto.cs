namespace WebTech.Auth.Models.Dtos;

public class BaseServiceDto
{
    public bool Success { get; set; } = true;
    public string ErrorMessage { get; set; }
}