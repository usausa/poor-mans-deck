namespace PoorMansDeck.Server;

using System.Windows.Forms;

public sealed partial class App : IDisposable
{
    private readonly NotifyIcon notifyIcon = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        // TODO
        var menu = new ContextMenuStrip();
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

    private void NotifyIconOnMouseDoubleClick(object? sender, MouseEventArgs e)
    {
        MainWindow?.Show();
    }

    private void OnExitClick(object? sender, EventArgs e)
    {
        Shutdown();
    }
}
