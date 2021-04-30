using Datalayer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using TicketsWebApp.Services;

namespace TicketsWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TicketsDbContext>();
            services.AddDbContext<IdentityUsersContext>(b =>
                            b.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;database=Test.IdentityServer4.EntityFramework;trusted_connection=yes;",
                            o => o.MigrationsAssembly("Datalayer"))); 

            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddCors();
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "Cookies";
                opt.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", o =>
            {
                o.SignInScheme = "Cookies";
                o.Authority = "https://localhost:5005";
                o.ClientId = "mvc-client";
                o.ResponseType = "code id_token";
                o.SaveTokens = true;
                o.ClientSecret = "MVCSecret";
                o.GetClaimsFromUserInfoEndpoint = true;
                o.Scope.Add("roles");
                o.ClaimActions.MapUniqueJsonKey("role", "role");
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "role"
                };
            });

            services.AddControllersWithViews();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            CheckDB(app);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Скрипт создания БД для тикетов
        /// </summary>
        /// <param name="app"></param>
        private void CheckDB(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                serviceScope.ServiceProvider.GetRequiredService<TicketsDbContext>().Database.EnsureCreated();
            
        }
    }
}
