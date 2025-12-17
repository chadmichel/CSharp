using System;

namespace LambdaPractice.Exercises
{
  // Events: attach simple lambda handlers
  public static class Exercise05_Events
  {
    public class Publisher
    {
      public event EventHandler<string>? Message;

      public void Raise(string msg)
      {
        Message?.Invoke(this, msg);
      }
    }

    // TODO: Attach two handlers using lambdas that write to the provided logger.
    // Return a disposable that detaches both handlers when disposed.
    public static IDisposable AttachHandlers(Publisher pub, Action<string> logger)
    {
      if (pub == null) throw new ArgumentNullException(nameof(pub));
      if (logger == null) throw new ArgumentNullException(nameof(logger));

      // Example handlers:
      // (s, m) => logger($"A: {m}")
      // (s, m) => logger($"B: {m}")

      EventHandler<string> h1 = (s, m) => { /* TODO */ };
      EventHandler<string> h2 = (s, m) => { /* TODO */ };

      // TODO: subscribe h1 and h2

      return new Detach(() =>
      {
        // TODO: unsubscribe h1 and h2
      });
    }

    private sealed class Detach : IDisposable
    {
      private readonly Action _dispose;
      private bool _done;
      public Detach(Action dispose) => _dispose = dispose;
      public void Dispose()
      {
        if (_done) return;
        _dispose();
        _done = true;
      }
    }
  }
}
