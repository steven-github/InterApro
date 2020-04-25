using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterApro.Web.Data
{

    /// <summary>
    /// Clase para inicializar datos en una base de datos nueva
    /// </summary>
    public class IdentityDataInitializer
    {
        /// <summary>
        /// Metodo para iniciar datos en general
        /// </summary>
        /// <param name="userManager">Manejador de los datos de usuarios</param>
        /// <param name="roleManager">Manejador de los datos de roles</param>
        public static void SeedData (UserManager<InterAproWebUser> userManager, RoleManager<InterAproWebRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        /// <summary>
        /// Creacion del usuario inicial
        /// </summary>
        /// <param name="userManager"></param>
        public static void SeedUsers (UserManager<InterAproWebUser> userManager)
        {
            if (userManager.FindByEmailAsync("jlmasis@gmail.com").Result == null)
            {
                InterAproWebUser user = new InterAproWebUser();
                user.UserName = "jlmasis@gmail.com";
                user.Email = "jlmasis@gmail.com";
                user.FirstName = "Jose Luis";
                user.LastName = "Mendez";
                user.Status = true;

                IdentityResult result = userManager.CreateAsync
                (user, "Admin01!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }

            if (userManager.FindByEmailAsync("jlmasis@hotmail.com").Result == null)
            {
                InterAproWebUser user = new InterAproWebUser();
                user.UserName = "jlmasis@hotmail.com";
                user.Email = "jlmasis@hotmail.com";
                user.FirstName = "Buyer";
                user.LastName = "Account";
                user.Status = true;
                user.ManagerId = userManager.FindByEmailAsync("jlmasis@gmail.com").Result.Id;

                IdentityResult result = userManager.CreateAsync
                (user, "Admin01!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Buyer").Wait();
                }
            }

            if (userManager.FindByEmailAsync("jlmasis@yahoo.com").Result == null)
            {
                InterAproWebUser user = new InterAproWebUser();
                user.UserName = "jlmasis@yahoo.com";
                user.Email = "jlmasis@yahoo.com";
                user.FirstName = "Finance 1";
                user.LastName = "Account";
                user.Status = true;

                IdentityResult result = userManager.CreateAsync
                (user, "Admin01!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "FinanceLevel_1").Wait();
                }
            }

            if (userManager.FindByEmailAsync("jlmasis@number8.com").Result == null)
            {
                InterAproWebUser user = new InterAproWebUser();
                user.UserName = "jlmasis@number8.com";
                user.Email = "jlmasis@number8.com";
                user.FirstName = "Finance 2";
                user.LastName = "Account";
                user.Status = true;

                IdentityResult result = userManager.CreateAsync
                (user, "Admin01!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "FinanceLevel_2").Wait();
                }
            }

            if (userManager.FindByEmailAsync("jose.mendez@wareaware.com").Result == null)
            {
                InterAproWebUser user = new InterAproWebUser();
                user.UserName = "jose.mendez@wareaware.com";
                user.Email = "jose.mendez@wareaware.com";
                user.FirstName = "Finance 3";
                user.LastName = "Account";
                user.Status = true;

                IdentityResult result = userManager.CreateAsync
                (user, "Admin01!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "FinanceLevel_3").Wait();
                }
            }
        }

        /// <summary>
        /// Creacion de los roles predefinidos
        /// </summary>
        /// <param name="roleManager"></param>
        public static void SeedRoles (RoleManager<InterAproWebRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Buyer").Result)
            {
                InterAproWebRole role = new InterAproWebRole();
                role.Name = "Buyer";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Manager").Result)
            {
                InterAproWebRole role = new InterAproWebRole();
                role.Name = "Manager";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("FinanceLevel_1").Result)
            {
                InterAproWebRole role = new InterAproWebRole();
                role.Name = "FinanceLevel_1";
                role.MinAmout = 1;
                role.MaxAmount = 100000;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("FinanceLevel_2").Result)
            {
                InterAproWebRole role = new InterAproWebRole();
                role.Name = "FinanceLevel_2";
                role.MinAmout = 100001;
                role.MaxAmount = 1000000;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("FinanceLevel_3").Result)
            {
                InterAproWebRole role = new InterAproWebRole();
                role.Name = "FinanceLevel_3";
                role.MinAmout = 1000001;
                role.MaxAmount = 10000000;
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

        }
    }
}
