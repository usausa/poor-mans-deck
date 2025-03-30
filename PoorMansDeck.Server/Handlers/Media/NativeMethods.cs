namespace PoorMansDeck.Server.Handlers.Media;

using System.Runtime.InteropServices;

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
internal static partial class NativeMethods
{
    private const int KEYEVENTF_EXTENDEDKEY = 1;
    private const int KEYEVENTF_KEYUP = 2;

    private const byte VK_MEDIA_NEXT_TRACK = 0xB0;
    private const byte VK_MEDIA_PREV_TRACK = 0xB1;
    private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;

    [LibraryImport("user32.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static partial void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

    [LibraryImport("user32.dll", EntryPoint = "MapVirtualKeyW")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static partial uint MapVirtualKey(uint uCode, uint uMapType);

    public static void PlayPause() => KeyEvent(VK_MEDIA_PLAY_PAUSE);

    public static void Prev() => KeyEvent(VK_MEDIA_PREV_TRACK);

    public static void Next() => KeyEvent(VK_MEDIA_NEXT_TRACK);

    private static void KeyEvent(byte key)
    {
        var scanCode = MapVirtualKey(key, 0);
        keybd_event(key, (byte)scanCode, KEYEVENTF_EXTENDEDKEY | 0, 0);
        keybd_event(key, (byte)scanCode, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
    }
}
