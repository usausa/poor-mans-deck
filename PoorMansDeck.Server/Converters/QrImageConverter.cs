namespace PoorMansDeck.Server.Converters;

using System;
using System.Windows.Media.Imaging;

using QRCoder;

[ValueConversion(typeof(string), typeof(BitmapImage))]
internal sealed class QrImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string text)
        {
            return null;
        }

        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L, true);
        using var png = new PngByteQRCode(data);
        var bytes = png.GetGraphic(20);
        using var ms = new MemoryStream(bytes);

        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = ms;
        image.EndInit();
        return image;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
