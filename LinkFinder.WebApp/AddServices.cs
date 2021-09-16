using LinkFinder.DbWorker;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinkFinder.WebApp
{
    public static class AddServices
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddEfRepository<LinkFinderDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DbConnection")));
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
