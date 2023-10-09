using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebTech.Auth.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    [EmailAddress]
    public override string Email
    {
        get => base.Email;
        set
        {
            base.Email = value;
            // Set the UserName to the value of Email when Email is set
            if (!string.IsNullOrWhiteSpace(value))
            {
                UserName = value;
            }
        }
    }
    public int Age { get; set; }
}