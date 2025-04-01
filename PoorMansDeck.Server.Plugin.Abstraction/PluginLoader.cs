namespace PoorMansDeck.Server.Plugin;

using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

public sealed class PluginLoader
{
    private readonly string[] modules;

    public PluginLoader(string[] modules)
    {
        this.modules = modules;
    }

    public void LoadPlugins(IServiceCollection services)
    {
        foreach (var module in modules)
        {
            var assembly = Assembly.LoadFrom(module);
            var pluginTypes = assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });
            foreach (var pluginType in pluginTypes)
            {
                var plugin = (IPlugin)Activator.CreateInstance(pluginType)!;
                plugin.Configure(services);
            }
        }
    }
}
