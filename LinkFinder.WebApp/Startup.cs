using LinkFinder.DbWorker;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace LinkFinder.WebApp
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
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddApplicationServices();
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
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller}/{id?}", defaults: new { controller = "Test", action = "Index" });
                routes.MapRoute("posteddata", "{controller}/{action}/{id?}", defaults: new { controller = "Test", action = "Index" });
            });
        }
    }
}
