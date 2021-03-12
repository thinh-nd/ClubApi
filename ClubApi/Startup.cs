using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubApi.Data;
using ClubApi.Data.Repositories;
using ClubApi.Filters;
using ClubApi.Models.Configurations;
using ClubApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;

namespace ClubApi
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
            services.AddControllers(option => option.Filters.Add<ExceptionFilter>())
                .AddNewtonsoftJson(settings => settings.SerializerSettings.NullValueHandling = NullValueHandling.Ignore);
            services.AddSwaggerGen();

            var elasticSetting = Configuration.GetSection(nameof(ElasticSetting)).Get<ElasticSetting>();
            services.AddSingleton<IElasticClient>(ElasticClientFactory.CreateElasticClient(elasticSetting));

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IClubRepository, ClubRepository>();
            services.AddScoped<IClubService, ClubService>();
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Family Tree API"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
