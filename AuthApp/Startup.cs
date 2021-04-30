using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using static Datalayer.IdentityUsersContext;
using Microsoft.Extensions.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;
using System;
using Datalayer;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AuthApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureIS(services);
            services.AddAuthentication()
                .AddJwtBearer("Bearer", o =>
                {
                    o.Authority = "https://localhot:5005";
                    o.RequireHttpsMetadata = false;
                    //было companyApi
                    o.Audience = "api";
                });

            services.AddCors();
            services.AddControllers();
            services.AddControllersWithViews();            
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            FillIs4DataBase(app);

            app.UseCors(b =>
            {
                b.AllowAnyMethod();
                b.AllowAnyHeader();
                b.AllowAnyOrigin();
            });

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void ConfigureIS(IServiceCollection services)
        {
            var is4connectionstring = Configuration.GetConnectionString("is4Storage");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<IdentityUsersContext>(b => 
                b.UseSqlServer(is4connectionstring, o=>o.MigrationsAssembly("Datalayer")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityUsersContext>();

            services.AddIdentityServer()                
                        .AddOperationalStore(o =>
                        {
                            o.ConfigureDbContext =
                            b => b.UseSqlServer(is4connectionstring,
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));                        
                        })
                        .AddConfigurationStore(o =>
                        {
                            o.ConfigureDbContext =
                            b => b.UseSqlServer(is4connectionstring,
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
                        })
                        //Ресурсы для работы приложения без БД
                        //.AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
                        //.AddTestUsers(InMemoryConfig.GetUsers())
                        //.AddInMemoryClients(InMemoryConfig.GetClients())
                        //.AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
                        //.AddInMemoryApiResources(InMemoryConfig.GetApiResources())
                        .AddAspNetIdentity<IdentityUser>()
                        .AddDeveloperSigningCredential();           
        }

        #region Скрипты инициализации и заполнения базы
        private void FillIs4DataBase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {

                if (serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.EnsureCreated())
                {
                    try
                    {
                        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                        context.Database.Migrate();
                        if (!context.Clients.Any())
                        {
                            foreach (var client in InMemoryConfig.GetClients())
                            {
                                context.Clients.Add(client.ToEntity());
                            }
                            context.SaveChanges();
                        }
                        if (!context.IdentityResources.Any())
                        {
                            foreach (var resource in InMemoryConfig.GetIdentityResources())
                            {
                                context.IdentityResources.Add(resource.ToEntity());
                            }
                            context.SaveChanges();
                        }
                        if (!context.ApiScopes.Any())
                        {
                            foreach (var apiScope in InMemoryConfig.GetApiScopes())
                            {
                                context.ApiScopes.Add(apiScope.ToEntity());
                            }
                            context.SaveChanges();
                        }
                        if (!context.ApiResources.Any())
                        {
                            foreach (var resource in InMemoryConfig.GetApiResources())
                            {
                                context.ApiResources.Add(resource.ToEntity());
                            }
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    try
                    {
                        var manager = serviceScope.ServiceProvider.GetRequiredService<SignInManager<IdentityUser>>();
                        var usercontext = serviceScope.ServiceProvider.GetRequiredService<IdentityUsersContext>();
                        usercontext.Database.Migrate();
                        if (!usercontext.Users.Any())
                        {
                            foreach (var user in InMemoryConfig.GetUsers())
                            {
                                var newUser = new IdentityUser
                                {
                                    UserName = user.Username,
                                    Id = user.SubjectId
                                };
                                newUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(newUser, user.Password);
                                var task = Task.Run(async () => { await manager.UserManager.CreateAsync(newUser); });
                                task.Wait();
                                foreach (var claim in user.Claims)
                                {
                                    usercontext.UserClaims.Add(
                                        new IdentityUserClaim<string>
                                        {
                                            ClaimType = claim.Type,
                                            ClaimValue = claim.Value,
                                            UserId = user.SubjectId
                                        });
                                    usercontext.SaveChanges();

                                }
                            }
                        }
                        if (!usercontext.UserRoles.Any())
                        {
                            usercontext.Roles.Add(new IdentityRole("manager"));
                            usercontext.Roles.Add(new IdentityRole("developer"));
                            usercontext.SaveChanges();
                        }
                        //if (!usercontext.UserClaims.Any())
                        //{
                        //    foreach (var user in InMemoryConfig.GetUsers())
                        //    {

                        //    }
                        //    usercontext.SaveChanges();
                        //}
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
        #endregion 

    }
}
