---
theme: apple-basic
info: |
  ## Introduction to C#
  A beginner-friendly introduction to C# programming
drawings:
  persist: false
transition: slide-left
title: Introduction to C#
mdc: true
---

<style src="./style.css"></style>

# Introduction to C#

A Modern, Powerful, Cross-Platform Language

<div class="pt-12">
  <span @click="$slidev.nav.next" class="px-2 py-1 rounded cursor-pointer" hover="bg-white bg-opacity-10">
    Press Space for next page <carbon:arrow-right class="inline"/>
  </span>
</div>

---

# What is C#?

<v-clicks>

- Created by Microsoft, first released in 2000
- Object-oriented, type-safe, modern language
- Runs on .NET — cross-platform (Windows, macOS, Linux)
- Used for web, desktop, mobile, games (Unity), cloud, and more
- Strongly typed with great tooling support

</v-clicks>

```csharp
// C# is expressive and readable
var message = "Hello, C#!";
Console.WriteLine(message);
// Output: Hello, C#!
```

---

# Creating a Console App

Use the .NET CLI to scaffold a new project instantly.

```bash
# Create a new console application
dotnet new console -n MyFirstApp

# Navigate into the project
cd MyFirstApp

# Run the application
dotnet run
```

<v-clicks>

- `dotnet new console` — creates a minimal console project
- A `Program.cs` file and a `.csproj` file are generated
- `dotnet run` compiles and executes in one step

</v-clicks>

---

# Creating a Project

```bash
# Create a project named "HelloWorld" in a folder called "HelloWorld"
dotnet new console -n HelloWorld

# Create a project named "HelloWorld" but put the files in a different folder
dotnet new console -n HelloWorld -o ./MyProjects/HelloWorld

# Short form (most people use this)
dotnet new console -o MyConsoleApp
```

---


# Anatomy of a Console App

The generated `Program.cs` uses top-level statements (C# 9+).

```csharp
// Program.cs — top-level statements (no class/Main needed)
Console.WriteLine("Hello, World!");
```

Traditional style (still valid):

```csharp
namespace MyFirstApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
```

---

# Reading Input from the Console

```csharp
Console.Write("Enter your name: ");
string? name = Console.ReadLine();

Console.WriteLine($"Hello, {name}!");

// Passing command-line arguments
// dotnet run -- Alice
string[] args = Environment.GetCommandLineArgs();
if (args.Length > 1)
{
    Console.WriteLine($"Argument received: {args[1]}");
}
```

<v-clicks>

- `Console.Write` — no newline at end
- `Console.WriteLine` — appends a newline
- `Console.ReadLine` — reads a line of user input

</v-clicks>

---

# Command line arguments are passed as an array of strings to `Main`:

```csharp

        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Command Line Arguments Example ===\n");

            // Way 1: Using command line arguments (passed when running the app)
            if (args.Length >= 2)
            {
                if (int.TryParse(args[0], out int num1) && int.TryParse(args[1], out int num2))
                {
                    Console.WriteLine($"Sum using command line arguments: {num1} + {num2} = {num1 + num2}");
                }
                else
                {
                    Console.WriteLine("Error: Both arguments must be valid integers.");
                }
            }
            else
            {
                Console.WriteLine("Not enough command line arguments provided.");
            }
        }
    }
```
---

# Command line arguments can also be passed using `dotnet run`:

```bash
# Example of passing command line arguments to the application
dotnet run -- 5 10
```

--- 

# Alternative Environment.args

```csharp
string? env1 = Environment.GetEnvironmentVariable("NUM1");
string? env2 = Environment.GetEnvironmentVariable("NUM2");

if (env1 != null && env2 != null &&
    int.TryParse(env1, out int n1) &&
    int.TryParse(env2, out int n2))
{
    Console.WriteLine($"Sum using environment variables: {n1} + {n2} = {n1 + n2}");
}
else
{
    Console.WriteLine("Environment variables NUM1 and/or NUM2 are not set or invalid.");
}
``` 

---

# Variables — Value Types

C# is statically typed: every variable has a declared type.

```csharp
int age = 30;           // 32-bit integer
long population = 8_000_000_000L; // 64-bit integer
double price = 9.99;    // 64-bit floating point
float temp = 98.6f;     // 32-bit floating point
decimal money = 19.95m; // High-precision decimal
bool isActive = true;   // true or false
char grade = 'A';       // Single Unicode character

Console.WriteLine($"Age: {age}, Price: {price}, Active: {isActive}");
// Output: Age: 30, Price: 9.99, Active: True
```

---

# Variables — Reference Types

```csharp
string greeting = "Hello, C#";  // Immutable text
int[] scores = { 95, 87, 76 };  // Array of ints
object anything = 42;           // Base type of all types

// Nullable reference types (C# 8+)
string? maybeNull = null;        // Explicitly nullable

// Constants — value cannot change
const double Pi = 3.14159;
Console.WriteLine(Pi);
// Output: 3.14159

// Read-only fields set once
readonly int maxRetries = 3;
```

---

# Type Inference with `var`

Use `var` when the type is obvious from context.

```csharp
var count = 10;               // int
var price = 4.99;             // double
var name = "Alice";           // string
var items = new List<string>(); // List<string>

// var requires initialization
// var x;  // ERROR — type cannot be inferred

// C# 9+ target-typed new
List<string> fruits = new();  // type inferred from left side
fruits.Add("Apple");
fruits.Add("Banana");

Console.WriteLine(fruits.Count); // Output: 2
```

---

# if / else — Basic

```csharp
int score = 75;

if (score >= 90)
{
    Console.WriteLine("Grade: A");
}
else if (score >= 80)
{
    Console.WriteLine("Grade: B");
}
else if (score >= 70)
{
    Console.WriteLine("Grade: C");
}
else
{
    Console.WriteLine("Grade: F");
}
// Output: Grade: C
```

---

# if / else — Ternary & Null Coalescing

```csharp
int temperature = 72;

// Ternary operator: condition ? valueIfTrue : valueIfFalse
string weather = temperature > 70 ? "Warm" : "Cool";
Console.WriteLine(weather); // Output: Warm

// Null coalescing: ?? returns right side when left is null
string? username = null;
string displayName = username ?? "Guest";
Console.WriteLine(displayName); // Output: Guest

// Null coalescing assignment: ??=
username ??= "DefaultUser";
Console.WriteLine(username); // Output: DefaultUser
```

---

# switch — Statement

```csharp
string day = "Monday";

switch (day)
{
    case "Monday":
    case "Tuesday":
    case "Wednesday":
    case "Thursday":
    case "Friday":
        Console.WriteLine("Weekday");
        break;
    case "Saturday":
    case "Sunday":
        Console.WriteLine("Weekend");
        break;
    default:
        Console.WriteLine("Unknown day");
        break;
}
// Output: Weekday
```

---

# switch — Expression (C# 8+)

Switch expressions are more concise and return a value.

```csharp
int month = 4;

string season = month switch
{
    12 or 1 or 2 => "Winter",
    3 or 4 or 5  => "Spring",
    6 or 7 or 8  => "Summer",
    9 or 10 or 11 => "Fall",
    _ => "Unknown"
};

Console.WriteLine(season); // Output: Spring

// Pattern matching in switch
object value = 3.14;
string kind = value switch
{
    int i    => $"Integer: {i}",
    double d => $"Double: {d}",
    string s => $"String: {s}",
    _        => "Other"
};
Console.WriteLine(kind); // Output: Double: 3.14
```

---

# for Loop

The `for` loop is ideal when you know the iteration count.

```csharp
// Basic for loop
for (int i = 0; i < 5; i++)
{
    Console.Write($"{i} ");
}
// Output: 0 1 2 3 4

Console.WriteLine();

// Count down
for (int i = 10; i >= 0; i -= 2)
{
    Console.Write($"{i} ");
}
// Output: 10 8 6 4 2 0

Console.WriteLine();

// Nested loops — multiplication table
for (int r = 1; r <= 3; r++)
    for (int c = 1; c <= 3; c++)
        Console.Write($"{r * c,3}");
```

---

# foreach Loop

Use `foreach` to iterate over any collection or array.

```csharp
string[] fruits = { "Apple", "Banana", "Cherry" };

foreach (string fruit in fruits)
{
    Console.WriteLine(fruit);
}
// Output:
// Apple
// Banana
// Cherry

// Works with any IEnumerable
var numbers = new List<int> { 10, 20, 30 };
foreach (var num in numbers)
{
    Console.Write($"{num} ");
}
// Output: 10 20 30
```

---

# while Loop

Repeat while a condition is true — check happens **before** each iteration.

```csharp
int count = 1;

while (count <= 5)
{
    Console.Write($"{count} ");
    count++;
}
// Output: 1 2 3 4 5

Console.WriteLine();

// Reading until sentinel value
string? input;
while ((input = Console.ReadLine()) != "quit")
{
    Console.WriteLine($"You typed: {input}");
}
Console.WriteLine("Goodbye!");
```

---

# do...while Loop

The body runs **at least once** — condition checked after each iteration.

```csharp
int number;

do
{
    Console.Write("Enter a positive number: ");
    string? raw = Console.ReadLine();
    number = int.Parse(raw ?? "0");
}
while (number <= 0);

Console.WriteLine($"You entered: {number}");

// Simpler example
int i = 1;
do
{
    Console.Write($"{i} ");
    i++;
}
while (i <= 5);
// Output: 1 2 3 4 5
```

---

# Loop Control — break & continue

```csharp
// break — exit the loop immediately
for (int i = 0; i < 10; i++)
{
    if (i == 5) break;
    Console.Write($"{i} ");
}
// Output: 0 1 2 3 4

Console.WriteLine();

// continue — skip the rest of this iteration
for (int i = 0; i < 10; i++)
{
    if (i % 2 == 0) continue; // skip even numbers
    Console.Write($"{i} ");
}
// Output: 1 3 5 7 9
```

---

# String Basics

Strings in C# are immutable sequences of Unicode characters.

```csharp
string first = "Hello";
string last  = "World";

// Concatenation
string full = first + ", " + last + "!";
Console.WriteLine(full); // Hello, World!

// Length and indexing
Console.WriteLine(full.Length);  // 13
Console.WriteLine(full[0]);      // H

// Equality is value-based
string a = "hello";
string b = "hello";
Console.WriteLine(a == b);      // True
Console.WriteLine(a.Equals(b)); // True
```

---

# String Interpolation & Verbatim Strings

```csharp
string name = "Alice";
int age = 30;

// String interpolation ($"...")
string intro = $"My name is {name} and I am {age} years old.";
Console.WriteLine(intro);
// Output: My name is Alice and I am 30 years old.

// Format numbers inside interpolation
double pi = Math.PI;
Console.WriteLine($"Pi = {pi:F4}"); // Output: Pi = 3.1416

// Verbatim string (@"...") — backslashes are literal
string path = @"C:\Users\Alice\Documents";
Console.WriteLine(path);
// Output: C:\Users\Alice\Documents
```

---

# String Methods — Case, Trim, Replace

```csharp
string text = "  Hello, World!  ";

Console.WriteLine(text.ToUpper());       // "  HELLO, WORLD!  "
Console.WriteLine(text.ToLower());       // "  hello, world!  "
Console.WriteLine(text.Trim());          // "Hello, World!"
Console.WriteLine(text.TrimStart());     // "Hello, World!  "
Console.WriteLine(text.TrimEnd());       // "  Hello, World!"

string sentence = "The cat sat on the mat";
Console.WriteLine(sentence.Replace("cat", "dog"));
// Output: The dog sat on the mat

Console.WriteLine(sentence.Contains("sat")); // True
Console.WriteLine(sentence.StartsWith("The")); // True
Console.WriteLine(sentence.EndsWith("mat"));   // True
```

---

# String Methods — Split & Join

```csharp
string csv = "Alice,Bob,Charlie,Diana";

// Split into an array
string[] names = csv.Split(',');
foreach (string n in names)
    Console.Write($"[{n}] ");
// Output: [Alice] [Bob] [Charlie] [Diana]

Console.WriteLine();

// Join back together
string joined = string.Join(" | ", names);
Console.WriteLine(joined);
// Output: Alice | Bob | Charlie | Diana

// Substring and IndexOf
string sentence = "Learning C# is fun!";
int idx = sentence.IndexOf("C#");
Console.WriteLine(sentence.Substring(idx, 2)); // C#
```

---

# StringBuilder — Efficient String Building

Use `StringBuilder` when concatenating many strings in a loop.

```csharp
using System.Text;

// Naive concatenation creates many intermediate strings
// StringBuilder avoids this
var sb = new StringBuilder();

for (int i = 1; i <= 5; i++)
{
    sb.Append($"Item {i}");
    if (i < 5) sb.Append(", ");
}

string result = sb.ToString();
Console.WriteLine(result);
// Output: Item 1, Item 2, Item 3, Item 4, Item 5

sb.Insert(0, "List: ");
sb.Replace("Item", "Entry");
Console.WriteLine(sb.ToString());
// Output: List: Entry 1, Entry 2, Entry 3, Entry 4, Entry 5
```

---

# Classes — Defining a Class

A class is a blueprint for creating objects.

```csharp
public class Animal
{
    // Fields (private by convention)
    private string _name;
    private int _age;

    // Constructor
    public Animal(string name, int age)
    {
        _name = name;
        _age  = age;
    }

    // Method
    public void Speak()
    {
        Console.WriteLine($"{_name} makes a sound.");
    }

    public override string ToString()
    {
        return $"Animal({_name}, age {_age})";
    }
}
```

---

# Classes — Properties

Properties expose data with controlled access.

```csharp
public class Person
{
    // Auto-implemented property
    public string Name { get; set; }

    // Property with validation
    private int _age;
    public int Age
    {
        get => _age;
        set
        {
            if (value < 0) throw new ArgumentException("Age cannot be negative");
            _age = value;
        }
    }

    // Read-only computed property
    public bool IsAdult => Age >= 18;
}

var p = new Person { Name = "Alice", Age = 30 };
Console.WriteLine($"{p.Name} is adult: {p.IsAdult}");
// Output: Alice is adult: True
```

---

# Classes — Methods & Overloading

```csharp
public class Calculator
{
    // Method overloading — same name, different parameters
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
    public int Add(int a, int b, int c) => a + b + c;

    // Optional parameters with default values
    public string Greet(string name, string greeting = "Hello")
    {
        return $"{greeting}, {name}!";
    }
}

var calc = new Calculator();
Console.WriteLine(calc.Add(2, 3));          // 5
Console.WriteLine(calc.Add(1.5, 2.5));      // 4
Console.WriteLine(calc.Add(1, 2, 3));       // 6
Console.WriteLine(calc.Greet("Alice"));     // Hello, Alice!
Console.WriteLine(calc.Greet("Bob", "Hi")); // Hi, Bob!
```

---

# Objects — Creating Instances

```csharp
public class Car
{
    public string Make  { get; set; }
    public string Model { get; set; }
    public int    Year  { get; set; }

    public Car(string make, string model, int year)
    {
        Make = make; Model = model; Year = year;
    }

    public string Description() => $"{Year} {Make} {Model}";
}

// Object creation with constructor
var car1 = new Car("Toyota", "Camry", 2023);

// Object initializer syntax
var car2 = new Car("Honda", "Civic", 2022);

Console.WriteLine(car1.Description()); // 2023 Toyota Camry
Console.WriteLine(car2.Description()); // 2022 Honda Civic

// Reference semantics — car3 points to same object as car1
var car3 = car1;
car3.Year = 2024;
Console.WriteLine(car1.Year); // 2024
```

---

# Objects — Inheritance

Classes can inherit from a base class using `:`.

```csharp
public class Shape
{
    public string Color { get; set; } = "Red";
    public virtual double Area() => 0;
    public override string ToString() => $"{Color} {GetType().Name}";
}

public class Circle : Shape
{
    public double Radius { get; set; }
    public Circle(double radius) { Radius = radius; }
    public override double Area() => Math.PI * Radius * Radius;
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Rectangle(double w, double h) { Width = w; Height = h; }
    public override double Area() => Width * Height;
}

Shape c = new Circle(5);
Shape r = new Rectangle(4, 6);
Console.WriteLine($"{c} area: {c.Area():F2}"); // Red Circle area: 78.54
Console.WriteLine($"{r} area: {r.Area():F2}"); // Red Rectangle area: 24.00
```

---

# Interfaces — Defining

An interface defines a contract — a set of members a class must implement.

```csharp
// Interface: only declarations, no implementation
public interface IGreeter
{
    string Greet(string name);      // Method signature
    string Language { get; }        // Property signature
}

public interface ILogger
{
    void Log(string message);
    void LogError(string error);
}

// Interfaces can have default implementations (C# 8+)
public interface IShape
{
    double Area();
    double Perimeter();
    string Describe() => $"Area={Area():F2}, Perimeter={Perimeter():F2}";
}
```

---

# Interfaces — Implementing

A class can implement multiple interfaces.

```csharp
public class EnglishGreeter : IGreeter
{
    public string Language => "English";
    public string Greet(string name) => $"Hello, {name}!";
}

public class SpanishGreeter : IGreeter
{
    public string Language => "Spanish";
    public string Greet(string name) => $"¡Hola, {name}!";
}

// Polymorphism — treat different types through a common interface
IGreeter[] greeters = { new EnglishGreeter(), new SpanishGreeter() };

foreach (IGreeter g in greeters)
{
    Console.WriteLine($"[{g.Language}] {g.Greet("Alice")}");
}
// Output:
// [English] Hello, Alice!
// [Spanish] ¡Hola, Alice!
```

---

# Interfaces — Real-World Pattern

```csharp
public interface IRepository<T>
{
    void Add(T item);
    T? GetById(int id);
    IEnumerable<T> GetAll();
}

public class InMemoryUserRepository : IRepository<string>
{
    private readonly List<string> _users = new();

    public void Add(string user) => _users.Add(user);

    public string? GetById(int id) =>
        id >= 0 && id < _users.Count ? _users[id] : null;

    public IEnumerable<string> GetAll() => _users;
}

IRepository<string> repo = new InMemoryUserRepository();
repo.Add("Alice");
repo.Add("Bob");

foreach (var user in repo.GetAll())
    Console.WriteLine(user);
// Output: Alice  Bob
```

---

# Putting It All Together

```csharp
public interface IAnimal { string Speak(); }

public class Dog : IAnimal
{
    public string Name { get; }
    public Dog(string name) { Name = name; }
    public string Speak() => $"{Name} says: Woof!";
}

public class Cat : IAnimal
{
    public string Name { get; }
    public Cat(string name) { Name = name; }
    public string Speak() => $"{Name} says: Meow!";
}

var animals = new List<IAnimal>
{
    new Dog("Rex"), new Cat("Whiskers"), new Dog("Buddy")
};

foreach (var animal in animals)
    Console.WriteLine(animal.Speak());
// Rex says: Woof!
// Whiskers says: Meow!
// Buddy says: Woof!
```

---

# What's Next?

<v-clicks>

- **Exception Handling** — `try`, `catch`, `finally`, custom exceptions
- **Generics** — type-safe reusable code (`List<T>`, `Dictionary<K,V>`)
- **LINQ** — powerful data querying over collections
- **Async / Await** — non-blocking asynchronous programming
- **Delegates & Events** — callbacks and the event-driven model
- **Records** — immutable data types (C# 9+)
- **Dependency Injection** — the backbone of .NET applications

</v-clicks>

---

# Summary

<v-clicks>

- C# apps start with `dotnet new console` and run with `dotnet run`
- Variables are statically typed; `var` infers the type
- `if/else`, `switch` control flow; switch expressions are concise
- `for`, `foreach`, `while`, `do...while` handle all looping needs
- Strings are immutable; use `StringBuilder` for heavy concatenation
- **Classes** define blueprints with fields, properties, and methods
- **Objects** are instances of classes; they share reference semantics
- **Interfaces** define contracts and enable polymorphism
- C# is modern, expressive, and keeps getting better with each version!

</v-clicks>

```csharp
Console.WriteLine("Happy coding in C#!");
```
