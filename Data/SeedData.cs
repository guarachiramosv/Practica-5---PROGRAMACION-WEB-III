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

            string farmaciaEmail = "farmacia@farmacia.com";
            string farmaciaPassword = "Farma123";

            var farmaciaExistente = await userManager.FindByEmailAsync(farmaciaEmail);

            if (farmaciaExistente == null)
            {
                var fUser = new ApplicationUser
                {
                    UserName = farmaciaEmail,
                    Email = farmaciaEmail,
                    Nombre = "Farmacia",
                    Apellido = "Personal",
                    EmailConfirmed = true
                };

                var resultadoF = await userManager.CreateAsync(fUser, farmaciaPassword);

                if (resultadoF.Succeeded)
                {
                    await userManager.AddToRoleAsync(fUser, "Farmaceutico");
                }
            }

            // Sembrar Categorías si no hay
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            if (!context.Categorias.Any())
            {
                context.Categorias.AddRange(
                    new Categoria { Nombre = "Analgésicos", Descripcion = "Alivio del dolor" },
                    new Categoria { Nombre = "Antibióticos", Descripcion = "Combate infecciones bacterianas" },
                    new Categoria { Nombre = "Vitaminas", Descripcion = "Suplementos nutricionales" }
                );
                await context.SaveChangesAsync();
            }

            // Sembrar Estantes si no hay
            if (!context.Estantes.Any())
            {
                context.Estantes.AddRange(
                    new Estante { Nombre = "A1", Ubicacion = "Pasillo Central", Descripcion = "Medicamentos de uso común" },
                    new Estante { Nombre = "B2", Ubicacion = "Sección Derecha", Descripcion = "Tratamientos específicos" },
                    new Estante { Nombre = "Refrigerados", Ubicacion = "Área de Frío", Descripcion = "Requiere temperatura controlada" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}