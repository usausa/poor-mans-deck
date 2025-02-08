using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyEvent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnPlayClick(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(2000);
            MediaControl.PlayPause();
        }

        private void OnPrevClick(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(2000);
            MediaControl.Prev();
        }

        private void OnNextClick(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(2000);
            MediaControl.Next();
        }
    }
}

public static partial class MediaControl
{
    private const int KEYEVENTF_EXTENDEDKEY = 1;
    private const int KEYEVENTF_KEYUP = 2;

    private const byte VK_MEDIA_NEXT_TRACK = 0xB0;
    private const byte VK_MEDIA_PREV_TRACK = 0xB1;
    private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;

    [LibraryImport("user32.dll")]
    public static partial void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

    [LibraryImport("user32.dll", EntryPoint = "MapVirtualKeyW")]
    public static partial uint MapVirtualKey(uint uCode, uint uMapType);

    public static void PlayPause() => Execute(VK_MEDIA_PLAY_PAUSE);

    public static void Prev() => Execute(VK_MEDIA_PREV_TRACK);

    public static void Next() => Execute(VK_MEDIA_NEXT_TRACK);

    private static void Execute(byte key)
    {
        var scanCode = MapVirtualKey(key, 0);
        keybd_event(key, (byte)scanCode, KEYEVENTF_EXTENDEDKEY | 0, 0);
        keybd_event(key, (byte)scanCode, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
    }
}
