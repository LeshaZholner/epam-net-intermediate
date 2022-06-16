using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    // todo: change this method to support cancellation token
    public static long Calculate(int n, CancellationToken cancellationToken)
    {
        long sum = 0;

        for (var i = 0; i < n && !cancellationToken.IsCancellationRequested; i++)
        {
            // i + 1 is to allow 2147483647 (Max(Int32)) 
            sum = sum + (i + 1);
            Thread.Sleep(10);
        }

        return sum;
    }

    public static Task<long> CalculateAsync(int n, CancellationToken cancellationToken)
    {
        var task = Task.Run(() =>
        {
            return Calculate(n, cancellationToken);
        });

        return task;
    }
}
