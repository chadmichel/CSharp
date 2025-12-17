using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaPractice.Exercises
{
  // Basics: get comfortable with lambda syntax and simple usage
  public static class Exercise01_Basics
  {
    // TODO: Return a function that adds 'a' to its input.
    // Example: CreateAdder(5)(10) => 15
    public static Func<int, int> CreateAdder(int a)
    {
      // Replace with a lambda that captures 'a'
      return x => x; // TODO
    }

    // TODO: Sort strings by length ascending using a lambda key selector.
    public static IEnumerable<string> SortByLength(IEnumerable<string> items)
    {
      if (items == null) return Enumerable.Empty<string>();
      // Replace with OrderBy using a lambda
      return items; // TODO
    }

    // TODO: Transform to upper-case using Select and a lambda.
    public static IEnumerable<string> TransformToUpper(IEnumerable<string> items)
    {
      if (items == null) return Enumerable.Empty<string>();
      // Replace with Select using a lambda
      return items; // TODO
    }

    // TODO: Return an Action that prints the provided prefix + message
    public static Action<string> MakePrinter(string prefix)
    {
      // Replace with a lambda that uses 'prefix'
      return _ => { /* no-op */ }; // TODO
    }

    // TODO: Expression-bodied version that multiplies two numbers
    // Replace body with a concise expression-bodied lambda assignment.
    public static Func<int, int, int> Multiply => (a, b) => a * b; // Example already done
  }
}
