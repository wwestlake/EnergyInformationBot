// See https://aka.ms/new-console-template for more information

using EnergyInforamtionBot;
using Job.Scheduler.Builder;
using Job.Scheduler.Scheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Main program
/// </summary>
public static class Program
{
    /// <summary>
    /// Creates a host and configures the swcheduler, then starts the job running
    /// </summary>
    public static void Main()
    {
        var host = CreateDefaultBuilder().Build();
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var eiaWorker = provider.GetRequiredService<EIAWorker>();
        var builder = new JobRunnerBuilder();
        var scheduler = new JobScheduler(builder);
        scheduler.ScheduleJob(eiaWorker);

        Thread.Sleep(Timeout.Infinite);

        scheduler.StopAsync();
    }

    /// <summary>
    /// Creates the host builder, configures the dependency injection and the appsettings file
    /// </summary>
    /// <returns>The hose builder</returns>
    private static IHostBuilder CreateDefaultBuilder()
    {

        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        IConfiguration config = configBuilder.Build();
        var connectionString = config.GetConnectionString("EIAConnect");

        var builder = Host.CreateDefaultBuilder().ConfigureAppConfiguration(app => {
            app.AddJsonFile("appsettings.json");
        }).ConfigureServices(services => {
            services.AddScoped<EIAWorker>();
            services.AddScoped<EIAClient>();
            services.AddDbContext<EIADataContext>(options => {
                options.UseSqlServer(connectionString);
            });
        });

        return builder;
    }
}





