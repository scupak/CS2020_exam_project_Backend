using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Implementations;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Core.Services.Validators.Implementations;
using Core.Services.Validators.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace UI.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                //Sql-lite database:
                services.AddDbContext<ClinicContext>(opt =>
                {
                    opt.UseSqlite("Data Source=ClinicAPI.db")
                        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

                }, ServiceLifetime.Transient);

            }
            else
            {
                //Azure SQL database:
                services.AddDbContext<ClinicContext>(opt =>
                {
                    opt.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
                        .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                }, ServiceLifetime.Transient);
            }

            services.AddTransient<IDbInitializer, DbInitializer>();
            services.AddScoped<IRepository<Doctor, int>, DoctorRepository>();
            services.AddScoped<IService<Doctor, int>, DoctorService>();
            services.AddScoped<IDoctorValidator, DoctorValidator>();

            services.AddScoped<IRepository<Patient, string>, PatientRepository>();
            services.AddScoped<IService<Patient, string>, PatientService>();
            services.AddScoped<IPatientValidator, PatientValidator>();

            //Set a max depth for Json to prevent endless nesting
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.MaxDepth = 5;
            });

            // Configure the default CORS policy.
            services.AddCors(options =>
            {
                options.AddPolicy(name: "ClinicBookingAppDev",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:44320", "https://localhost:44320")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                options.AddPolicy(name: "ClinicBookingAppAllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("Placeholder for azure app url", "Placeholder for azure app url")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // Register the Swagger generator using Swashbuckle.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ClinicBookingAPI",
                    Description = "A simple example ASP.NET Core Web API"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("ClinicBookingAppDev");
                //Initialize the database
                using var scope = app.ApplicationServices.CreateScope();
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<ClinicContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer.Initialize(dbContext);
            }
            else
            {
                app.UseCors("ClinicBookingAppAllowSpecificOrigins");
                using var scope = app.ApplicationServices.CreateScope();
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<ClinicContext>();
                dbContext.Database.EnsureCreated();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve SwaggerUI, specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClinicAPI V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
