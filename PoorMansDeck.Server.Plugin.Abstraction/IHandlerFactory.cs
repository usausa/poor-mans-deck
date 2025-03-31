namespace PoorMansDeck.Server.Plugin;

public interface IHandlerFactory
{
    IEnumerable<IHandler> CreateHandlers();
}
