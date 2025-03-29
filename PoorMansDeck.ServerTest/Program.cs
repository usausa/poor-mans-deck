using Grpc.Net.Client;

using PoorMansDeck.Contract;

using ProtoBuf.Grpc.Client;

var channel = GrpcChannel.ForAddress("http://localhost:9928");

var client = channel.CreateGrpcService<IHelloService>();
var reply = await client.HelloAsync(new HelloRequest { Name = "うさうさ" }).ConfigureAwait(false);
Console.WriteLine(reply.Message);
