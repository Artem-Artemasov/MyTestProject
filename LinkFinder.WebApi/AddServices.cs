using AutoMapper;
using LinkFinder.DbWorker;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.Services;
using LinkFinder.WebApi.Services.Filters;
using LinkFinder.WebApi.Services.Mappers.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkFinder.WebApi
{
    public static class AddServices
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddEfRepository<LinkFinderDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<DatabaseWorker>();
            services.AddScoped<LinkParser>();
            services.AddScoped<LinkConverter>();
            services.AddScoped<RequestService>();
            services.AddScoped<LinkValidator>();
            services.AddScoped<HtmlCrawler>();
            services.AddScoped<SitemapCrawler>();
            services.AddScoped<CrawlerApp>();
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
