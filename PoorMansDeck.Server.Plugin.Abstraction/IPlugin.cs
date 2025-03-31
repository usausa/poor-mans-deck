namespace PoorMansDeck.Server.Plugin;

using Microsoft.Extensions.DependencyInjection;

public interface IPlugin
{
    void Configure(IServiceCollection services);
}
