namespace PoorMansDeck.Server.Security;

// TODO setting
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class AuthenticationSettings
{
    public const string Audience = nameof(Audience);

    public const string Issuer = nameof(Issuer);

    public const string SecretKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";

    public const int ExpireDays = 7;
}
