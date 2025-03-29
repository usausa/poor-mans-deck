namespace PoorMansDeck.Server;

using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using PoorMansDeck.Server.Services;

using Serilog;

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
        // TODO port ? / setting ?
        builder.WebHost.ConfigureKestrel((_, serverOptions) =>
        {
            serverOptions.ListenAnyIP(5000, listenOptions =>
            {
                // listenOptions.UseHttps("certificate.pfx", "password");
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
            });
        });

        builder.Services.AddGrpc();

        return builder;
    }

    //--------------------------------------------------------------------------------
    // Components
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureComponents(this WebApplicationBuilder builder)
    {
        //builder.ConfigureContainer(new SmartServiceProviderFactory(), x => ConfigureContainer(builder.Configuration, x));

        //RestConfig.Default.UseJsonSerializer(static config =>
        //{
        //    config.Converters.Add(new Template.WindowsApp.Helpers.DateTimeConverter());
        //    config.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        //    config.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        //});

        // TODO plugins & configure plugins ?

        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddSingleton<App>();

        return builder;
    }

    //--------------------------------------------------------------------------------
    // Startup
    //--------------------------------------------------------------------------------
    public static void MapGrpcService(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<HelloService>();
    }

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

        app.Services.GetRequiredService<App>().Run();
    }
}
