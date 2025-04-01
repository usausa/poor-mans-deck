namespace PoorMansDeck.Server;

using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PoorMansDeck.Server.Hubs;
using PoorMansDeck.Server.Plugin;
using PoorMansDeck.Server.Views;

using Scalar.AspNetCore;

using Serilog;

using Smart.Windows.Hosting;
using Smart.Windows.Resolver;

public static class ApplicationExtensions
{
    //--------------------------------------------------------------------------------
    // System
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureSystemDefaults(this WebApplicationBuilder builder)
    {
        Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        return builder;
    }

    //--------------------------------------------------------------------------------
    // Logging
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureLoggingDefaults(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Services.AddSerilog(options =>
        {
            options.ReadFrom.Configuration(builder.Configuration);
        });

        return builder;
    }

    //--------------------------------------------------------------------------------
    // API
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddSignalR();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddOpenApi();
        }

        return builder;
    }

    public static void MapApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseAuthorization();

        // TODO Image?
        //app.UseStaticFiles();

        app.UseRouting();

        app.MapControllers();
        app.MapHub<DeckHub>("/deck");
        app.MapGet("/", () => "Poor man's deck");
    }

    //--------------------------------------------------------------------------------
    // Components
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureComponents(this WebApplicationBuilder builder)
    {
        // TODO plugins & configure plugins ?

        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddWpf<App>();

        // Plugins
        var pluginLoader = new PluginLoader(
            builder.Configuration.GetSection("Plugins").Get<string[]>()!
                .Select(static x => Path.Combine(AppContext.BaseDirectory, x))
                .ToArray());
        pluginLoader.LoadPlugins(builder.Services);

        return builder;
    }

    //--------------------------------------------------------------------------------
    // Startup
    //--------------------------------------------------------------------------------

    public static void LogStartupInformation(this IHost app)
    {
        var log = app.Services.GetRequiredService<ILogger<Program>>();
        var environment = app.Services.GetRequiredService<IHostEnvironment>();
        ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);

        log.InfoStartup();
        log.InfoStartupSettingsRuntime(RuntimeInformation.OSDescription, RuntimeInformation.FrameworkDescription, RuntimeInformation.RuntimeIdentifier);
        log.InfoStartupSettingsGC(GCSettings.IsServerGC, GCSettings.LatencyMode, GCSettings.LargeObjectHeapCompactionMode);
        log.InfoStartupSettingsThreadPool(workerThreads, completionPortThreads);
        log.InfoStartupApplication(environment.ApplicationName, typeof(Program).Assembly.GetName().Version);
        log.InfoStartupEnvironment(environment.EnvironmentName, environment.ContentRootPath);
    }

    public static void RunApplication(this IHost app)
    {
        ResolveProvider.Default.Provider = app.Services;

        app.Run();
    }
}
