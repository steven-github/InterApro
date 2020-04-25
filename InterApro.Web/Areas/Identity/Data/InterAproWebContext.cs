using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterApro.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InterApro.Web.Data
{
    /// <summary>
    /// Clase del contexto de base de datos para el manejo de autenticacion de el sistema
    /// </summary>
    public class InterAproWebContext : IdentityDbContext<InterAproWebUser, InterAproWebRole, string>
    {
        public InterAproWebContext(DbContextOptions<InterAproWebContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
