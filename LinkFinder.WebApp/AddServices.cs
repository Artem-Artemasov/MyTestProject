using LinkFinder.DbWorker;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LinkFinder.WebApp
{
    public static class AddServices
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddEfRepository<LinkFinderDbContext>(options => options.UseSqlServer(@"Server=DESKTOP-BFO0R26; Database=LinkFinder; Trusted_Connection=True"));
            services.AddScoped<DatabaseWorker>();
            services.AddScoped<LinkParser>();
            services.AddScoped<LinkConverter>();
            services.AddScoped<RequestService>();
            services.AddScoped<LinkValidator>();
            services.AddScoped<HtmlCrawler>();
            services.AddScoped<SitemapCrawler>();
            services.AddScoped<CrawlerApp>();
        }
    }
}
