namespace PoorMansDeck.Server.Plugin;

internal ref struct ParameterEnumerator
{
    private ReadOnlySpan<char> remaining;

    public ParameterEnumerator(ReadOnlySpan<char> text)
    {
        remaining = text;
        Key = default;
        Value = default;
    }

    public ReadOnlySpan<char> Key { get; private set; }

    public ReadOnlySpan<char> Value { get; private set; }

    public bool MoveNext()
    {
        remaining = remaining.TrimStart();
        if (remaining.IsEmpty)
        {
            return false;
        }

        // Key
        var equalIndex = remaining.IndexOf('=');
        if (equalIndex == -1)
        {
            return false;
        }

        Key = remaining[..equalIndex];

        // Value
        remaining = remaining[(equalIndex + 1)..];
        if (remaining.IsEmpty)
        {
            Value = default;
            return true;
        }

        if (remaining[0] == '"')
        {
            remaining = remaining[1..];
            var length = 0;
            var escape = false;
            for (; length < remaining.Length; length++)
            {
                var c = remaining[length];
                if (escape)
                {
                    escape = false;
                }
                else if (c == '\\')
                {
                    escape = true;
                }
                else if (c == '"')
                {
                    break;
                }
            }

            Value = remaining[..length];
            remaining = length + 1 < remaining.Length ? remaining[(length + 1)..] : default;
        }
        else
        {
            var spaceIndex = remaining.IndexOf(' ');
            if (spaceIndex == -1)
            {
                Value = remaining;
                remaining = default;
            }
            else
            {
                Value = remaining[..spaceIndex];
                remaining = remaining[spaceIndex..];
            }
        }

        return true;
    }
}
