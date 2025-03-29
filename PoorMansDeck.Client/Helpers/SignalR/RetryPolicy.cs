namespace PoorMansDeck.Client.Helpers.SignalR;

using Microsoft.AspNetCore.SignalR.Client;

public class RetryPolicy : IRetryPolicy
{
    private readonly TimeSpan delay;

    public RetryPolicy(TimeSpan delay)
    {
        this.delay = delay;
    }

    public TimeSpan? NextRetryDelay(RetryContext retryContext) => delay;
}
