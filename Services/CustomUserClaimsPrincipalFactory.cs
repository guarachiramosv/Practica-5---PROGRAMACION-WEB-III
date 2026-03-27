using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using practica5PR.Models;
using System.Security.Claims;

namespace practica5PR.Services
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FirstName", user.Nombre ?? ""));
            identity.AddClaim(new Claim("LastName", user.Apellido ?? ""));
            identity.AddClaim(new Claim("FullName", $"{user.Nombre} {user.Apellido}"));
            return identity;
        }
    }
}
