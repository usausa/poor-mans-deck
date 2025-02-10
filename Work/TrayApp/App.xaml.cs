namespace TrayApp;

using System.Windows;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly NotifyIcon notifyIcon = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        var menu = new ContextMenuStrip();
        menu.Items.Add("Exit", null, OnExitClick);
        notifyIcon.Icon = new Icon("App.ico");
        notifyIcon.Text = "テスト";
        notifyIcon.ContextMenuStrip = menu;
        notifyIcon.MouseDoubleClick += NotifyIconOnMouseDoubleClick;
        notifyIcon.Visible = true;

        MainWindow = new MainWindow();
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
