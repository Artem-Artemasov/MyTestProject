using AutoMapper;
using LinkFinder.DbWorker;
using LinkFinder.DbWorker.Interfaces;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.Logic.Filters;
using LinkFinder.WebApi.Logic.Mappers.Profiles;
using LinkFinder.WebApi.Logic.Response.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkFinder.WebApi
{
    public static class AddServices
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddEfRepository<LinkFinderDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<DatabaseWorker>();
            services.AddScoped<LinkFinder.Logic.Services.LinkParser>();
            services.AddScoped<LinkConverter>();
            services.AddScoped<RequestService>();
            services.AddScoped<LinkValidator>();
            services.AddScoped<HtmlCrawler>();
            services.AddScoped<SitemapCrawler>();
            services.AddScoped<ICrawlerApp, CrawlerApp>();
            services.AddScoped<ResultService>();
            services.AddScoped<TestService>();
            services.AddScoped<ResultFilter>();

            var mapper = GetMapper();
            services.AddSingleton(mapper);

        }

        private static IMapper GetMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EnitityToDtoMapperProfile());
            });

            return mappingConfig.CreateMapper();
        }
    }
}
