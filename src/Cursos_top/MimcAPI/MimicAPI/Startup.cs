using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Repositories.Contracts;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MimicAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            #region AutoMapper-Config
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Base-Config
            services.AddDbContext<MimicContext>(opt =>
            {
                opt.UseSqlite("Data Source=Database\\MimicAPI.db");

            });
            #endregion

            services.AddScoped<IWordRepository, WordRepository>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                //c.ResolveConflictingActions(ApiDescription => ApiDescription.First());

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Mimic Api",
                    Description = "API for game of mimic. It to create, read, update, Delete words of data base."
                });

                // Set the comments path for the Swagger JSON and UI.  
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStatusCodePages();
            
            app.UseSwagger();

            IApplicationBuilder applicationBuilder = app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });




        }

    }
}





