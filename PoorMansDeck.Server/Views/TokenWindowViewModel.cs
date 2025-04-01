namespace PoorMansDeck.Server.Views;

using PoorMansDeck.Server.Security;

public sealed class TokenWindowViewModel
{
    public string Token { get; set; }

    public TokenWindowViewModel()
    {
        // TODO
        Token = TokenHelper.Generate();
    }
}
