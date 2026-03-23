using System;

namespace LambdaPractice.Exercises
{
  // Delegates: Func, Action, Predicate, and higher-order helpers
  public static class Exercise04_Delegates
  {
    // TODO: Compose two functions: (x => f(g(x)))
    public static Func<T, T> Compose<T>(Func<T, T> f, Func<T, T> g)
    {
      // Replace with a lambda that composes f and g
      return x => x; // TODO
    }

    // TODO: Apply a unary function twice to a value
    public static T ApplyTwice<T>(T value, Func<T, T> f)
    {
      // Replace with invoking f twice via a lambda or directly
      return value; // TODO
    }

    // TODO: If predicate holds, perform action on value and return true; else false
    public static bool DoIf<T>(T value, Predicate<T> predicate, Action<T> action)
    {
      // Implement using lambdas and built-in delegates
      return false; // TODO
    }
  }
}
