using Duende.IdentityServer.Models;

namespace WebTech.Auth;

public class IdentityConfig
{
    public const string User = "User";
    public const string Admin = "Admin";
    public const string Support = "Support";
    public const string SuperAdmin = "SuperAdmin";
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
}