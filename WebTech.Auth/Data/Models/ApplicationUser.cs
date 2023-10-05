using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebTech.Auth.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    [EmailAddress]
    public override string Email { get; set; }
    public int Age { get; set; }
}