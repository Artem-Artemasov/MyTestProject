using LinkFinder.DbWorker;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddControllersWithViews();
            services.AddEfRepository<LinkFinderDbContext>(options => options.UseSqlServer(@"Server=DESKTOP-BFO0R26; Database=LinkFinder; Trusted_Connection=True"));
            services.AddScoped<DatabaseWorker>();
            services.AddScoped<LinkParser>();
            services.AddScoped<LinkConverter>();
            services.AddScoped<RequestService>();
            services.AddScoped<LinkValidator>();
            services.AddScoped<HtmlCrawler>();
            services.AddScoped<SitemapCrawler>();
            services.AddScoped<WebCrawlerApp>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Test}/{action=Index}/{id?}");
            });
        }
    }
}
