using LinkFinder.DbWorker;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.Filters;
using LinkFinder.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LinkFinder.WebApi
{
    public static class AddServices
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddEfRepository<LinkFinderDbContext>(options => options.UseSqlServer(@"Server=DESKTOP-ACD27RC\LOCALDB; Database=LinkFinder;User ID=user_sql;Password=q1w2e3r4"));
            services.AddScoped<DatabaseWorker>();
            services.AddScoped<LinkParser>();
            services.AddScoped<LinkConverter>();
            services.AddScoped<RequestService>();
            services.AddScoped<LinkValidator>();
            services.AddScoped<HtmlCrawler>();
            services.AddScoped<SitemapCrawler>();
            services.AddScoped<CrawlerApp>();
            services.AddScoped<ResultsService>();
            services.AddScoped<TestsService>();
            services.AddScoped<ResultsFilter>();
        }
    }
}
