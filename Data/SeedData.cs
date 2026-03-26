using Microsoft.AspNetCore.Identity;
using practica5PR.Models;

namespace practica5PR.Data
{
    public static class SeedData
    {
        public static async Task InicializarAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Administrador", "Farmaceutico", "Cliente" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string adminEmail = "admin@farmacia.com";
            string adminPassword = "Admin123";

            var adminExistente = await userManager.FindByEmailAsync(adminEmail);

            if (adminExistente == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nombre = "Administrador",
                    Apellido = "Principal",
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(admin, adminPassword);

                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Administrador");
                }
            }
        }
    }
}