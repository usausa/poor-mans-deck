namespace PoorMansDeck.Server.Plugin;

using System.Globalization;

public static class ParameterBinder
{
    public static void Bind<T>(T target, ReadOnlySpan<char> text)
    {
        var type = typeof(T);

        var enumerator = new ParameterEnumerator(text);
        while (enumerator.MoveNext())
        {
            var pi = type.GetProperty(enumerator.Key.ToString());
            if (pi == null)
            {
                // Ignore
                continue;
            }

            var converter = pi.GetCustomAttributes(true)
                .OfType<ConverterAttribute>()
                .FirstOrDefault();
            var value = converter is not null
                ? converter.FromString(enumerator.Value)
                : FromString(enumerator.Value, pi.PropertyType);
            pi.SetValue(target, value);
        }
    }

    private static object FromString(ReadOnlySpan<char> value, Type type)
    {
        if (value.IsEmpty)
        {
            return Activator.CreateInstance(type)!;
        }

        if (type == typeof(string))
        {
            return value.ToString();
        }

        return Convert.ChangeType(value.ToString(), Nullable.GetUnderlyingType(type) ?? type, CultureInfo.InvariantCulture);
    }
}
