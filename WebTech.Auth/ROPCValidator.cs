using Duende.IdentityServer.Validation;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using WebTech.Auth.Data.Models;
using System.Security.Claims;
using IdentityModel;

namespace WebTech.Auth;

public class ROPCValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ROPCValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var email = context.UserName;
        var user = await _userManager.FindByEmailAsync(email);

        if(user != null && await _userManager.CheckPasswordAsync(user, context.Password))
        {
            if (!user.EmailConfirmed)
            {
                var customData = new Dictionary<string, object>
                {
                    { "user_id", user.Id },
                    { "email", user.Email }
                };

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Email not confirmed", customData);
                return;
            }

            context.Result = new GrantValidationResult(
                subject: user.Id.ToString(),
                authenticationMethod: "pwd",
                claims: GetUserClaims(user));

            return;
        }

        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
    }

    private static IEnumerable<Claim> GetUserClaims(ApplicationUser user)
    {
        return new[]
        {
            new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
            new Claim(JwtClaimTypes.PreferredUserName, user.Email)
        };
    }
}