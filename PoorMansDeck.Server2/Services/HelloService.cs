namespace PoorMansDeck.Server2.Services;

using PoorMansDeck.Contract;

public class HelloService : IHelloService
{
    public Task<HelloResponse> HelloAsync(HelloRequest request)
    {
        return Task.FromResult(new HelloResponse { Message = $"Hello {request.Name}" });
    }
}
