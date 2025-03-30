namespace PoorMansDeck.Server.Handlers.Media;

using NAudio.CoreAudioApi;

// TODO
#pragma warning disable CA1822
public sealed class MediaHandler
{
    public void VolumeUp()
    {
        using var deviceEnum = new MMDeviceEnumerator();
        var device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        device.AudioEndpointVolume.VolumeStepUp();
    }

    public void VolumeDown()
    {
        using var deviceEnum = new MMDeviceEnumerator();
        var device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        device.AudioEndpointVolume.VolumeStepDown();
    }

    public void Mute()
    {
        using var deviceEnum = new MMDeviceEnumerator();
        var device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        if (!device.AudioEndpointVolume.Mute)
        {
            device.AudioEndpointVolume.Mute = true;
        }
    }

    // ReSharper disable once IdentifierTypo
    public void Unmute()
    {
        using var deviceEnum = new MMDeviceEnumerator();
        var device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        if (device.AudioEndpointVolume.Mute)
        {
            device.AudioEndpointVolume.Mute = false;
        }
    }
}
