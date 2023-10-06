using IdentityModel;
using Microsoft.AspNetCore.Mvc;

namespace WebTech.Auth.Controllers.Base;

[Controller]
public abstract class BaseController : ControllerBase
{
    protected string GetUserId()
        => User
            .Claims
            .SingleOrDefault(x => x.Type == JwtClaimTypes.Subject)
            ?.Value;
}
