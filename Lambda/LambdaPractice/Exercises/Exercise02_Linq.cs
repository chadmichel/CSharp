using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaPractice.Exercises
{
  // LINQ practice with lambdas
  public static class Exercise02_Linq
  {
    public record Person(int Id, string Name, int Age);
    public record Item(string Name, decimal Price);

    // TODO: Return only adults (Age >= 18)
    public static IEnumerable<Person> FilterAdults(IEnumerable<Person> people)
    {
      if (people == null) return Enumerable.Empty<Person>();
      // Replace with Where using a lambda
      return people; // TODO
    }

    // TODO: Project only the names of people
    public static IEnumerable<string> ProjectNames(IEnumerable<Person> people)
    {
      if (people == null) return Enumerable.Empty<string>();
      // Replace with Select using a lambda
      return Enumerable.Empty<string>(); // TODO
    }

    // TODO: Find first person by Id, or null if missing
    public static Person? FirstOrNullById(IEnumerable<Person> people, int id)
    {
      if (people == null) return null;
      // Replace with FirstOrDefault using a predicate lambda
      return null; // TODO
    }

    // TODO: Compute total price of items using Sum
    public static decimal TotalPrice(IEnumerable<Item> items)
    {
      if (items == null) return 0m;
      // Replace with Sum using a selector lambda
      return 0m; // TODO
    }

    // TODO: Build a comma-separated string of names using Aggregate
    public static string JoinNames(IEnumerable<Person> people)
    {
      if (people == null) return string.Empty;
      // Replace with Aggregate (or string.Join with Select)
      return string.Empty; // TODO
    }
  }
}
