using NAudio.CoreAudioApi;

using var deviceEnum = new MMDeviceEnumerator();
var device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
device.AudioEndpointVolume.VolumeStepUp();
device.AudioEndpointVolume.VolumeStepDown();
device.AudioEndpointVolume.Mute = !device.AudioEndpointVolume.Mute;
device.AudioEndpointVolume.Mute = !device.AudioEndpointVolume.Mute;
