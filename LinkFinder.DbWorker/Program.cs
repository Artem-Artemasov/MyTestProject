using System;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace LinkFinder.DbWorker
{
    class Program
    {
        static async Task Main(string[] args)
        {

            using IHost host = CreateHostBuilder(args).Build();
/*            var linkConsoleApp = host.Services.GetService<CrawlerConsoleApp>();*/
/*            linkConsoleApp.Start();*/
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                var connectionString = Environment.GetEnvironmentVariable("DbConnectionString");
                services.AddEfRepository<LinkFinderDbContext>(options => options.UseSqlServer(connectionString));
/*                services.AddScoped<CrawlerConsoleApp>();*/
                services.AddScoped<DatabaseWorker>();
                services.AddScoped<RequestService>();
                services.AddScoped<LinkParser>();
                services.AddScoped<LinkConverter>();
                services.AddScoped<LinkValidator>();
                services.AddScoped<HtmlCrawler>();
                services.AddScoped<SitemapCrawler>();
            })
            .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Error));
    }
}
