namespace PoorMansDeck.Server.Plugin;

[AttributeUsage(AttributeTargets.Property)]
public abstract class ConverterAttribute : Attribute
{
    public abstract object FromString(ReadOnlySpan<char> text);
}
