using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using WatchGuard.RoverApi.Exercise.Actions;
using WatchGuard.RoverApi.Exercise.Helpers;
using WatchGuard.RoverApi.Exercise.Interfaces;

namespace WatchGuard.RoverApi.Exercise
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            await host.Services.GetRequiredService<RetrievePhotosAction>().ExecuteAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configuation) =>
        {
            configuation.Sources.Clear();
            configuation.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddOptions();
            services.Configure<ApiOptions>(hostContext.Configuration.GetSection("ApiOptions"));
            services.AddTransient<IEarthDateHelper, EarthDateHelper>();
            services.AddTransient<RetrievePhotosAction>();
        });
    }
}
