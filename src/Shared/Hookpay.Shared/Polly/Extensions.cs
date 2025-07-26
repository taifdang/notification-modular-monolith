
namespace Hookpay.Shared.Polly;

using global::Polly;
public static class Extensions
{
    //ref: https://www.codeproject.com/Articles/5378791/How-to-Use-Polly-In-Csharp-Easily-Handle-Faults-An
    public static T RetryStrategyOnFailure<T>(this object source, Func<T> action,int retryCount = 3)
    {
       var retryPolicy = Policy
            .Handle<Exception>()
            .Retry(retryCount, (exception, retryAttempt, context) =>
            {
                Console.WriteLine($"Retry attempt: {retryAttempt}");
                Console.WriteLine($"Exception: {exception.Message}");
            });
       
        return retryPolicy.Execute(action);
    }

}
