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
using MimicAPI.Helpers.Swagger;
using MimicAPI.V1.Repositories.Contracts;
using System.Linq;

namespace MimicAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
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

            //services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<IWordRepository, WordRepository>();

            services.AddControllers();


           


            //services.AddApiVersioning(cfg =>
            //{
            //    cfg.ReportApiVersions = true;
            //    cfg.AssumeDefaultVersionWhenUnspecified = true;
            //    cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            //});

            services.AddSwaggerGen(c =>
            {
                //c.ResolveConflictingActions(ApiDescription => ApiDescription.First());

                c.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Versioned Api v1.0",
                    Version = "v1.0"
                });

                //c.SwaggerDoc("v2.0", new Microsoft.OpenApi.Models.OpenApiInfo
                //{
                //    Title = "Versioned Api v2.0",
                //    Version = "v2.0"
                //});

                //c.OperationFilter<ApiVersionOperationFilter>();

                //c.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                //    // would mean this action is unversioned and should be included everywhere
                //    if (actionApiVersionModel == null)
                //    {
                //        return true;
                //    }
                //    if (actionApiVersionModel.DeclaredApiVersions.Any())
                //    {
                //        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                //    }
                //    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
                //});
            });




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }

            //app.UseMvc();

            app.UseRouting();

            app.UseStatusCodePages();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.RoutePrefix = string.Empty;
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "MimicAPI - v2.0");

                c.SwaggerEndpoint("/v1/swagger.json", "MimicAPI - v1.0");

            });


            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });




        }

    }
}





