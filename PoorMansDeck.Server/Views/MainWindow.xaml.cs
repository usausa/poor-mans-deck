namespace PoorMansDeck.Server.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // TODO
    private void Window_Closing(object sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}
