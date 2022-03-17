using Concert.Domain.Entities;
using Concert.Infrastructure.Service;
using IdentityServer.In_Memory;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<AuthDbService>(s => s.UseSqlServer(Configuration["ConnectionStrings:Concert"],
               sql => sql.MigrationsAssembly(migrationsAssembly)));
            services.AddIdentity<UserModel, IdentityRole>(opt => opt.SignIn.RequireConfirmedEmail = true)
                 .AddEntityFrameworkStores<AuthDbService>()
                    .AddDefaultTokenProviders();
            services.AddIdentityServer()
               .AddTestUsers(IdentityConfiguration.TestUsers)
               .AddConfigurationStore(options =>
               {
                   options.ConfigureDbContext = b => b.UseSqlServer(Configuration["ConnectionStrings:Concert"],
                       sql => sql.MigrationsAssembly(migrationsAssembly));
               })
               .AddOperationalStore(options =>
               {
                   options.ConfigureDbContext = b => b.UseSqlServer(Configuration["ConnectionStrings:Concert"],
                       sql => sql.MigrationsAssembly(migrationsAssembly));
               }).AddAspNetIdentity<UserModel>().AddDeveloperSigningCredential();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseIdentityServer();

        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityConfiguration.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityConfiguration.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in IdentityConfiguration.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
