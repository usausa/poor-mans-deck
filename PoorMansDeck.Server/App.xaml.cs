namespace PoorMansDeck.Server;

public sealed partial class App : IDisposable
{
    private readonly ILogger<App> log;

    private readonly System.Windows.Forms.NotifyIcon notifyIcon = new();

    public App(ILogger<App> log)
    {
        this.log = log;

        Current.DispatcherUnhandledException += (_, ea) => HandleException(ea.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, ea) => HandleException((Exception)ea.ExceptionObject);

        InitializeComponent();
    }

    private void HandleException(Exception ex)
    {
        log.ErrorUnknownException(ex);

        System.Windows.MessageBox.Show(ex.ToString(), "Unknown error.", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        // TODO
        var menu = new System.Windows.Forms.ContextMenuStrip();
        menu.Items.Add("Exit", null, OnExitClick);
        notifyIcon.Icon = new Icon("App.ico");
        notifyIcon.Text = "Test";
        notifyIcon.ContextMenuStrip = menu;
        notifyIcon.MouseDoubleClick += NotifyIconOnMouseDoubleClick;
        notifyIcon.Visible = true;

        MainWindow = new MainWindow();
    }

    public void Dispose()
    {
        notifyIcon.Dispose();
    }

    private void NotifyIconOnMouseDoubleClick(object? sender, System.Windows.Forms.MouseEventArgs e)
    {
        MainWindow?.Show();
    }

    private void OnExitClick(object? sender, EventArgs e)
    {
        Shutdown();
    }
}
