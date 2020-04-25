using System;
using InterApro.Web.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(InterApro.Web.Areas.Identity.IdentityHostingStartup))]
namespace InterApro.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        /// <summary>
        /// Configuracion de los servicios de autenticacion y autorizacion de la aplicacion
        /// </summary>
        /// <param name="builder">Constructor de la estructura inicial en memoria de la aplicacion</param>
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<InterAproWebContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("InterAproWebContextConnection")));

                services.AddDefaultIdentity<InterAproWebUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<InterAproWebRole>()
                    .AddEntityFrameworkStores<InterAproWebContext>();
            });
        }
    }
}