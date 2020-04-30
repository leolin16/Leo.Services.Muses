using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Leo.Services.Muses.Data;
using Leo.Services.Muses.Interfaces;
using Leo.Services.Muses.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace Leo.Services.Muses
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _env = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment _env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<MusesDbContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllers()
                    .AddNewtonsoftJson(options => 
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver()
                    ) // earlier added, to be dafault
                    // this is for getting the naming convention from camelcase to initial caption case
                    //.AddJsonOptions(o => {
                    //    if(o.SerializerSettings.ContractResolver != null) {
                    //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                    //        castedResolver.NamingStrategy = null;
                    //    }
                    //});
                    // .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    // this is for specifying input/output formatter
                    .AddMvcOptions(o => o.OutputFormatters.Add(
                        new XmlDataContractSerializerOutputFormatter()
                       ));
			services.AddScoped<IMusesRepository, MusesRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStatusCodePages();
            app.UseCookiePolicy();

            // app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // no routing conventions setup here, intend to use attribute-based routing
            });

			// AutoMapper.Mapper.Initialize(cfg => {
			// 	cfg.CreateMap<Entities.Singer, Models.SingerWithoutSongsDto>();
			// 	cfg.CreateMap<Entities.Singer, Models.SingerDto>();
			// 	cfg.CreateMap<Models.SingerDtoForEdit, Entities.Singer>();
			// 	cfg.CreateMap<Entities.Singer, Models.SingerDtoForEdit>(); // for patch
			// 	cfg.CreateMap<Entities.Song, Models.SongWithoutSingersDto>();
			// 	cfg.CreateMap<Entities.Song, Models.SongDto>();
			// 	cfg.CreateMap<Models.SongDtoForEdit, Entities.Song>();
			// 	cfg.CreateMap<Entities.Song, Models.SongDtoForEdit>(); // for patch
            //     cfg.CreateMap<Entities.Criticism, Models.CriticismWithoutSingerDto>();
			// 	cfg.CreateMap<Entities.Criticism, Models.CriticismDto>();
			// 	cfg.CreateMap<Models.CriticismDtoForEdit, Entities.Criticism>();
			// 	cfg.CreateMap<Entities.Criticism, Models.CriticismDtoForEdit>(); // for patch
			// });
        }
    }
}
