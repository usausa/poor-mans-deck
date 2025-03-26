namespace PoorMansDeck.Server;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // TODO
    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}
