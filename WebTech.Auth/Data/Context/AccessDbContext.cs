using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebTech.Auth.Data.Models;

namespace WebTech.Auth.Data.Context;

public class AccessDbContext : IdentityDbContext<ApplicationUser>
{
    public AccessDbContext(DbContextOptions<AccessDbContext> options) 
        : base(options)  
    {
    }

    public DbSet<ExternalToken> ExternalTokens { get; set; }
}