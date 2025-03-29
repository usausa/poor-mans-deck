namespace PoorMansDeck.Server;

using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ProtoBuf.Grpc.Server;

using PoorMansDeck.Contract;
using PoorMansDeck.Server.Services;

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
    // gRPC
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureGrpcService(this WebApplicationBuilder builder)
    {
        builder.Services.AddCodeFirstGrpc();
        builder.Services.AddSingleton<IHelloService, HelloService>();

        return builder;
    }

    public static void MapGrpcService(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<IHelloService>();
    }

    //--------------------------------------------------------------------------------
    // Components
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureComponents(this WebApplicationBuilder builder)
    {
        // TODO plugins & configure plugins ?

        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddWpf<App>();

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
