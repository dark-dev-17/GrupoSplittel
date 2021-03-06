﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionPersonal.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionPersonal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddDbContext<GPS_Logic.Data.DireccionContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("Default")); });
            //services.AddDbContext<GPS_Logic.Data.SociedadContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("Default")); });
            //services.AddTransient

            //services.Configure<GPDataInformation.GestionPersonal>(options => {
            //    options.StringConnectionDb = Configuration.GetConnectionString("Default");

            //});
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:89").AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;

            });
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IConfiguration>(Configuration);


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
                    app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
            app.UseCors("ApiCorsPolicy");
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=DoLogin}/{id?}");
            });

            // Configuramos Rotativa indicándole el Path RELATIVO donde se
            // encuentran los archivos de la herramienta wkhtmltopdf.
            Rotativa.AspNetCore.RotativaConfiguration.Setup(env);
        }
    }
}
