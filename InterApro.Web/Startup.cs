using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using InterApro.Web.Data;
using Microsoft.EntityFrameworkCore;
using InterApro.Models.Mail;
using InterApro.Business.Mail;
using System.IO;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using InterApro.Database;
using System.Globalization;

namespace InterApro.Web
{
    public class Startup
    {
        IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          
                services.AddDbContext<InterAproDBContext>(options => 
                {
                    options.UseSqlServer(Configuration.GetConnectionString("InterAproWebContextConnection"),
                        assembly => assembly.MigrationsAssembly(typeof(InterAproDBContext).Assembly.FullName));
                    
                });

                //Configuracion para enviar correos electronicos
                var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            //Obtencion la lista de plantillas de correo del folder de plantillas
            emailConfig.Templates = Directory.GetFiles(_env.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Templates" + Path.DirectorySeparatorChar.ToString());

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<InterAproWebUser> userManager, RoleManager<InterAproWebRole> roleManager)
        {
            var cultureInfo = new CultureInfo("es-CR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            //Inicializacion de datos para usuario
            IdentityDataInitializer.SeedData(userManager, roleManager);
        }
    }
}
