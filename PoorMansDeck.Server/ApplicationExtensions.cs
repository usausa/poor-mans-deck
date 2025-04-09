namespace PoorMansDeck.Server;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using PoorMansDeck.Server.Hubs;
using PoorMansDeck.Server.Plugin;
using PoorMansDeck.Server.Security;
using PoorMansDeck.Server.Services;
using PoorMansDeck.Server.Views;

using Scalar.AspNetCore;

using Serilog;

public static class ApplicationExtensions
{
    //--------------------------------------------------------------------------------
    // Logging
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Services.AddSerilog(options =>
        {
            options.ReadFrom.Configuration(builder.Configuration);
        });

        return builder;
    }

    //--------------------------------------------------------------------------------
    // Security
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureSecurity(this WebApplicationBuilder builder)
    {
        // TODO
        builder.Services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = AuthenticationSettings.Issuer,
                    ValidAudience = AuthenticationSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationSettings.SecretKey))
                };
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

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHub<DeckHub>("/deck");
        app.MapGet("/", () => "Poor man's deck");
    }

    //--------------------------------------------------------------------------------
    // Components
    //--------------------------------------------------------------------------------

    public static WebApplicationBuilder ConfigureComponents(this WebApplicationBuilder builder)
    {
        // TODO
        // Services
        builder.Services.AddSingleton<SecurityService>();
        builder.Services.AddSingleton<DeckService>();
        builder.Services.AddSingleton<ImageService>();

        // Window
        builder.Services.AddSingleton<MainWindow>();

        // Plugins
        var pluginLoader = new PluginLoader(
            builder.Configuration.GetSection("Plugins").Get<string[]>()!
                .Select(static x => Path.Combine(AppContext.BaseDirectory, x))
                .ToArray());
        pluginLoader.LoadPlugins(builder.Services);

        return builder;
    }
}
