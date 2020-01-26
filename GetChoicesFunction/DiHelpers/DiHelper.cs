using Adventure.Core;
using Adventure.Core.DiHelpers;
using Adventure.Core.Models;
using Adventure.Persistence.DiHelpers;
using GetChoicesFunction.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace GetChoicesFunction.DiHelpers
{
    public static class DiHelper
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var envname = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"local.settings.{envname}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            services.AddOptions();
            services.Configure<DocumentDbSettings>(configuration.GetSection(Constants.DOCUMENT_DB_SETTINGS));
            
            services.AddScoped<IItemService, ItemService>();

            services.RegisterModule<PersistenceCatalog>();

            return services.BuildServiceProvider();
        }
    }
}
