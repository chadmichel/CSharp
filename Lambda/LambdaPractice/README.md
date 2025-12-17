# C# Lambda Practice

Welcome! This repo is a focused set of exercises to learn and practice C# lambdas, delegates, closures, and LINQ.

How to use:

- Open the files under the `Exercises/` folder.
- Implement TODOs in each exercise file.
- Use `dotnet run` to make sure the project builds.
- Optionally, add your own quick prints in `Program.cs` to try things out.

## Exercises Overview

1. Basics — syntax and simple usage

- File: [Exercises/Exercise01_Basics.cs](Exercises/Exercise01_Basics.cs)
  - Implement simple `Func` and `Action` lambdas
  - Sort and transform collections using `OrderBy`, `Select`
  - Use expression-bodied members

2. LINQ — filtering, projection, aggregation

- File: [Exercises/Exercise02_Linq.cs](Exercises/Exercise02_Linq.cs)
  - Filter with `Where`
  - Project with `Select`
  - Find elements with `FirstOrDefault` and `Any`
  - Aggregate with `Sum` and `Aggregate`

3. Closures — capturing variables

- File: [Exercises/Exercise03_Closures.cs](Exercises/Exercise03_Closures.cs)
  - Build closures that capture outer variables
  - Verify counters are independent
  - Understand capture pitfalls and how to avoid them

4. Delegates — `Func<>`, `Action<>`, `Predicate<>`

- File: [Exercises/Exercise04_Delegates.cs](Exercises/Exercise04_Delegates.cs)
  - Compose functions
  - Pass lambdas as parameters
  - Implement small higher-order helpers

5. Events — quick lambda handlers

- File: [Exercises/Exercise05_Events.cs](Exercises/Exercise05_Events.cs)
  - Attach/detach event handlers with lambdas
  - Observe handler invocation order

---

## Hints

- Prefer `Func<T>` and `Action` where appropriate.
- For LINQ, remember to add `using System.Linq;`.
- Use small helper variables to keep lambdas readable.
- Use `var` to simplify generic delegate declarations.

## Run

```bash
cd /Users/chadmichel/Education/CSharp/Lambda/LambdaPractice
 dotnet run
```

You can temporarily add quick prints in `Program.cs` to try your answers while you work. When you’re done, you can remove those prints to keep the project clean.
