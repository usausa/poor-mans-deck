namespace PoorMansDeck.Server;

using System.Diagnostics;
using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;

using PoorMansDeck.Server.Security;
using PoorMansDeck.Server.Views;

using Smart.Windows.Resolver;

#pragma warning disable CA1001
public sealed partial class App
#pragma warning restore CA1001
{
    private readonly IHost host;

    private readonly ILogger<App> log;

    private readonly NotifyIcon notifyIcon = new();

    //--------------------------------------------------------------------------------
    // Constructor
    //--------------------------------------------------------------------------------

    public App()
    {
        InitializeComponent();

        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        host = CreateHost();

        log = host.Services.GetRequiredService<ILogger<App>>();
        ResolveProvider.Default.Provider = host.Services;

        var environment = host.Services.GetRequiredService<IHostEnvironment>();
        ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);

        log.InfoStartup();
        log.InfoStartupSettingsRuntime(RuntimeInformation.OSDescription, RuntimeInformation.FrameworkDescription, RuntimeInformation.RuntimeIdentifier);
        log.InfoStartupSettingsGC(GCSettings.IsServerGC, GCSettings.LatencyMode, GCSettings.LargeObjectHeapCompactionMode);
        log.InfoStartupSettingsThreadPool(workerThreads, completionPortThreads);
        log.InfoStartupApplication(environment.ApplicationName, typeof(App).Assembly.GetName().Version);
        log.InfoStartupEnvironment(environment.EnvironmentName, environment.ContentRootPath);

        SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;

        Current.DispatcherUnhandledException += (_, ea) => HandleException(ea.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, ea) => HandleException((Exception)ea.ExceptionObject);
    }

    private static WebApplication CreateHost()
    {
        var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

        // Log
        builder.ConfigureLogging();
        // Security
        builder.ConfigureSecurity();
        // API
        builder.ConfigureApi();
        // Components
        builder.ConfigureComponents();

        var app = builder.Build();

        // API
        app.MapApi();

        return app;
    }

    //--------------------------------------------------------------------------------
    // Lifecycle
    //--------------------------------------------------------------------------------

    // ReSharper disable once AsyncVoidMethod
    protected override async void OnStartup(StartupEventArgs e)
    {
        // TODO
        var menu = new System.Windows.Forms.ContextMenuStrip();
        menu.Items.Add("Token", null, OnTokenClick);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("Exit", null, OnExitClick);
        notifyIcon.Icon = new Icon("App.ico");
        notifyIcon.Text = "Booting";
        notifyIcon.ContextMenuStrip = menu;
        notifyIcon.MouseDoubleClick += NotifyIconOnMouseDoubleClick;
        notifyIcon.Visible = true;

        // TODO
        MainWindow = host.Services.GetRequiredService<MainWindow>();

        // TODO slow ?
        await host.StartAsync().ConfigureAwait(false);

        // TODO log & visual effect ?
        notifyIcon.Text = "Test";
    }

    // ReSharper disable once AsyncVoidMethod
    protected override async void OnExit(ExitEventArgs e)
    {
        await host.StopAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
        host.Dispose();

        notifyIcon.Dispose();
    }

    //--------------------------------------------------------------------------------
    // Event
    //--------------------------------------------------------------------------------

    private void HandleException(Exception ex)
    {
        log.ErrorUnknownException(ex);

        System.Windows.MessageBox.Show(ex.ToString(), "Unknown error.", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private static void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        // TODO
        switch (e.Reason)
        {
            case SessionSwitchReason.SessionLock:
                Debug.WriteLine("* Session is locked.");
                break;
            case SessionSwitchReason.SessionUnlock:
                Debug.WriteLine("* Session is unlocked.");
                break;
        }
    }

    //--------------------------------------------------------------------------------
    // Handler
    //--------------------------------------------------------------------------------

    private void NotifyIconOnMouseDoubleClick(object? sender, System.Windows.Forms.MouseEventArgs e)
    {
        MainWindow?.Show();
    }

    private void OnTokenClick(object? sender, EventArgs e)
    {
        var vm = host.Services.GetRequiredService<TokenWindowViewModel>();
        // TODO ?
        vm.Token = TokenHelper.Generate();

        var window = host.Services.GetRequiredService<TokenWindow>();
        window.Show();
    }

    private void OnExitClick(object? sender, EventArgs e)
    {
        Shutdown();
    }
}
