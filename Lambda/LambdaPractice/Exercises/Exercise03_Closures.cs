using System;
using System.Collections.Generic;

namespace LambdaPractice.Exercises
{
  // Closures: capturing variables inside lambdas
  public static class Exercise03_Closures
  {
    // TODO: Return a list of independent counters.
    // Each function, when called, should increment and return its own count starting from 0.
    // Example: var counters = MakeCounters(3);
    // counters[0](); // 1, then 2, then 3...
    // counters[1](); // 1 independent of the first
    public static List<Func<int>> MakeCounters(int howMany)
    {
      var result = new List<Func<int>>();
      for (int i = 0; i < howMany; i++)
      {
        int count = 0; // Ensure each lambda captures its own variable
                       // Replace with a lambda that captures 'count'
        result.Add(() => 0); // TODO
      }
      return result;
    }

    // TODO: Return a list of multiplier functions based on factors.
    // For each factor f in factors, create a function x => f * x
    public static List<Func<int, int>> BuildMultipliers(IEnumerable<int> factors)
    {
      var result = new List<Func<int, int>>();
      if (factors == null) return result;
      foreach (var f in factors)
      {
        var factor = f; // avoid capture pitfalls
                        // Replace with a lambda that captures 'factor'
        result.Add(x => x); // TODO
      }
      return result;
    }
  }
}
