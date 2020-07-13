
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quote.Api.Config;
using Quote.Api.Models;
using Quote.Api.Models.Domain;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Quote.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options => {
                options.AddPolicy("AllowAccessToAll", builder =>
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin()
                );
            });
            //services.AddMvc().AddFluentValidation();

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));


            // Configure swagger
            services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                // Specify two versions 
                options.SwaggerDoc("v1.0",
                    new Info()
                    {
                        Version = "v1",
                        Title = "Quote.Api V1",
                        Description = "Documentation Quotation API",
                        Contact = new Contact
                        {
                            Email = "admin@dirmod.com",
                            Name = "DIRMOD",
                            Url = "https://dirmod.com"
                        }
                    });
                options.SwaggerDoc("v2.0",
                   new Info()
                   {
                       Version = "v2",
                       Title = "Api.CurrencyConversion V1",
                       Description = "Documentation Currency Conversion API",
                       Contact = new Contact
                       {
                           Email = "admin@dirmod.com",
                           Name = "DIRMOD",
                           Url = "https://dirmod.com"
                       }
                   });


                // This make replacement of v{version:apiVersion} to real version of corresponding swagger doc.
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                options.DocInclusionPredicate((version, desc) =>
                {
                    var versions = desc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == version);
                });        


                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });


            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider)
        {
            if (!env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Execute pending migration
            //if (!env.IsDevelopment())
            //{
            //    provider.GetService<ApplicationDbContext>().Database.Migrate();
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Api.Quote V1");
                c.SwaggerEndpoint("/swagger/v2.0/swagger.json", "Api.Quote V2");

            });


            app.UseAuthentication();
            app.UseCors("AllowAccessToAll");


          
            // global cors policy
            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials());


            app.UseMvc(
                route=> {
                    route.MapRoute(
                        name: "default",
                        template: "{controller=Default}/{action=Index}/{id?}"
                        );
                }
                
                );
        }
    }
}
