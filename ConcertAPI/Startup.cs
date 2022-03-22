using Concert.Application.Interface;
using Concert.Domain.Entities;
using Concert.Infrastructure.Service;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ConcertAPI
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
            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddControllers();

            services.AddIdentity<UserModel, IdentityRole>(opt => opt.SignIn.RequireConfirmedEmail = true)
                 .AddEntityFrameworkStores<AuthDbService>()
                    .AddDefaultTokenProviders();
            services.AddDbContext<AuthDbService>(s => s.UseSqlServer(Configuration["ConnectionStrings:Concert"],
                sql=>sql.MigrationsAssembly(migrationsAssembly)));

            services.AddScoped<Client>();
            services.AddSingleton<MappingConfig>();
            services.AddScoped<IdentityServices>();
            services.AddScoped<EmailService>();
            services.AddScoped<IAuth, TwoFactorAuthService>();

            services.AddAuthentication(options=>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:34085";
                options.RequireHttpsMetadata = false;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = PathString.Empty;
                options.AccessDeniedPath = PathString.Empty;

            });
            services.Configure<IdentityOptions>(options => 
            {
                options.Password.RequiredLength = 6;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            });


            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("ConcertAPISpecification", new OpenApiInfo()
                {
                    Title = "Concert Api",
                    Version = "1.0",
                    Contact = new OpenApiContact()
                    {
                        Email = "adeolaaderibigbe09@gmail.com",
                        Name = "Adeola Aderibigbe",
                        Url = new Uri("https://twitter.com/addiexandria")

                    }

                });
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                setupAction.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                //Locate the XML file being generated by ASP.NET...
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                //... and tell Swagger to use those XML comments.
                setupAction.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/ConcertAPISpecification/swagger.json", "Concert API");
                setupAction.RoutePrefix = string.Empty;
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
       
        
    }
}
