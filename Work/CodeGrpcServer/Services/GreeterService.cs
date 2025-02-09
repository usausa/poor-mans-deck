namespace CodeGrpcServer.Services;

using CodeGrpcContract;

public class HelloService : IHelloService
{
    public Task<HelloResponse> HelloAsync(HelloRequest request)
    {
        return Task.FromResult(new HelloResponse { Message = $"Hello {request.Name}" });
    }
}
