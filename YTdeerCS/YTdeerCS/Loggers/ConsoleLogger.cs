using Microsoft.Extensions.Logging;

namespace YTdeerCS.Loggers;

public class ConsoleLogger<T> : ILogger<T>
{
    IDisposable? ILogger.BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    bool ILogger.IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        Console.WriteLine(message);
    }
}