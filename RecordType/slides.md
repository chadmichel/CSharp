---
theme: apple-basic
colorSchema: light
info: |
  ## C# Record Types
  Record types are a powerful feature in C# that provide value-based equality, immutability, and concise syntax for data carriers.
drawings:
  persist: false
transition: slide-left
title: C# Record Types
mdc: true
---

<style src="./style.css"></style>

# C# Record Types

Value-based equality, immutability, and concise syntax for data carriers

<div class="pt-12">
  <span @click="$slidev.nav.next" class="px-2 py-1 rounded cursor-pointer" hover="bg-white bg-opacity-10">
    Press Space for next page <carbon:arrow-right class="inline"/>
  </span>
</div>

---

# What is a Record?

A `record` is a special kind of type optimized for **data** rather than behavior

<v-clicks>

- Introduced in **C# 9** (2020)
- Automatically generates: equality, `ToString`, `GetHashCode`, and more
- Primary use case: **immutable data carriers** — DTOs, value objects, events
- Comes in two flavors: `record class` (reference type) and `record struct` (value type)
- Reduces boilerplate drastically compared to a hand-written class

</v-clicks>

---

# The Problem Records Solve

Writing a proper data class by hand is verbose and error-prone

```csharp
// Without records — lots of boilerplate
public class PersonOld
{
    public string Name { get; }
    public int Age { get; }

    public PersonOld(string name, int age) { Name = name; Age = age; }

    public override bool Equals(object? obj) =>
        obj is PersonOld p && Name == p.Name && Age == p.Age;

    public override int GetHashCode() => HashCode.Combine(Name, Age);

    public override string ToString() => $"Person {{ Name = {Name}, Age = {Age} }}";
}
```

<v-clicks>

```csharp
// With records — one line!
public record Person(string Name, int Age);
```

</v-clicks>

---

# Minimal Record Syntax

The **positional record** — the most concise form

```csharp
public record Person(string Name, int Age);
```

That single line generates:
- A constructor: `new Person("Alice", 30)`
- Read-only properties: `Name`, `Age`
- `Equals` / `==` based on property values
- `GetHashCode` based on property values
- `ToString` that prints all properties
- A `Deconstruct` method
- A `with` expression clone mechanism

---

# Creating Record Instances

```csharp
public record Person(string Name, int Age);

// Positional constructor
var alice = new Person("Alice", 30);

// Named arguments (clearer for many properties)
var bob = new Person(Name: "Bob", Age: 25);

// Access properties
Console.WriteLine(alice.Name);  // Alice
Console.WriteLine(alice.Age);   // 30

// Deconstruct
var (name, age) = alice;
Console.WriteLine($"{name} is {age}"); // Alice is 30
```

---

# Value-Based Equality

Records use **value equality** — two records are equal if their data matches

```csharp
public record Person(string Name, int Age);

var a = new Person("Alice", 30);
var b = new Person("Alice", 30);
var c = new Person("Bob",   25);

Console.WriteLine(a == b);            // True  — same data
Console.WriteLine(a == c);            // False — different data
Console.WriteLine(a.Equals(b));       // True
Console.WriteLine(ReferenceEquals(a, b)); // False — different objects
```

<v-clicks>

Compare to a plain `class` — two instances with the same data would **not** be equal by default

</v-clicks>

---

# ToString — Automatic and Useful

Records generate a `ToString()` that prints all properties

```csharp
public record Person(string Name, int Age);
public record Address(string Street, string City);
public record Employee(string Name, int Age, Address Office);

var emp = new Employee("Alice", 30, new Address("1 Main St", "NYC"));

Console.WriteLine(emp);
// Employee { Name = Alice, Age = 30,
//            Office = Address { Street = 1 Main St, City = NYC } }
```

<v-clicks>

Nested records are printed recursively — great for debugging and logging

</v-clicks>

---

# with Expressions — Non-Destructive Mutation

Create a **modified copy** without changing the original

```csharp
public record Person(string Name, int Age, string City);

var alice = new Person("Alice", 30, "NYC");

var olderAlice  = alice with { Age = 31 };
var movedAlice  = alice with { City = "LA" };
var renamed     = alice with { Name = "Alicia", City = "Boston" };

Console.WriteLine(alice);       // Person { Name = Alice, Age = 30, City = NYC }
Console.WriteLine(olderAlice);  // Person { Name = Alice, Age = 31, City = NYC }
Console.WriteLine(movedAlice);  // Person { Name = Alice, Age = 30, City = LA }

// Original is always unchanged
```

---

# with Expressions — Deep Copy?

`with` does a **shallow copy** — nested reference types are shared

```csharp
public record Address(string City);
public record Person(string Name, Address Home);

var alice = new Person("Alice", new Address("NYC"));
var copy  = alice with { Name = "Alicia" };

// Both share the same Address object
Console.WriteLine(ReferenceEquals(alice.Home, copy.Home)); // True

// Mutating the nested Address would affect both
// (but record Address is immutable by default, so this is safe)
```

<v-clicks>

Since record properties are immutable, this shallow copy is usually fine in practice

</v-clicks>

---

# Immutability by Default

Record properties generated from positional syntax are **init-only**

```csharp
public record Person(string Name, int Age);

var alice = new Person("Alice", 30);

alice.Name = "Bob"; // COMPILE ERROR — init-only property
```

```csharp
// 'init' allows setting during object initialization only
var alice2 = new Person("Alice", 30)
{
    // Can't set here either — constructor already ran
};
```

<v-clicks>

Immutability prevents accidental mutation — safer for concurrent code and predictable state

</v-clicks>

---

# Mutable Records (Optional)

You can opt into mutability — but consider if you really need it

```csharp
public record MutablePerson
{
    public string Name { get; set; }   // mutable
    public int Age    { get; set; }    // mutable
}

var p = new MutablePerson { Name = "Alice", Age = 30 };
p.Name = "Bob";   // allowed
p.Age  = 31;      // allowed

Console.WriteLine(p); // MutablePerson { Name = Bob, Age = 31 }
```

<v-clicks>

Mutable records are unusual — if you need mutation, a regular `class` might be clearer

</v-clicks>

---

# record class vs record struct

Two flavors — choose based on value vs reference semantics

```csharp
// record class — reference type (default, heap-allocated)
public record class PersonClass(string Name, int Age);

// record struct — value type (stack-allocated)
public record struct PersonStruct(string Name, int Age);

// 'record' alone defaults to 'record class'
public record Person(string Name, int Age); // same as record class
```

```csharp
var rc = new PersonClass("Alice", 30);
var rs = new PersonStruct("Alice", 30);

// Both have value equality, ToString, with expressions
Console.WriteLine(rc == new PersonClass("Alice", 30)); // True
Console.WriteLine(rs == new PersonStruct("Alice", 30)); // True
```

---

# record class vs record struct — Key Differences

| Feature | record class | record struct |
|---------|--------------|---------------|
| Memory | Heap | Stack |
| Default value | `null` | Zero-initialized |
| Can be `null` | Yes | Only with `?` |
| Inheritance | Yes | No |
| Boxing | No | When cast to `object` |
| `with` copies | Shallow heap copy | Full value copy |
| Best for | Larger data, hierarchy | Small data, performance |

---

# Inheritance with record class

Records support single-level inheritance like classes

```csharp
public record Animal(string Name, int Age);

public record Dog(string Name, int Age, string Breed)
    : Animal(Name, Age);

public record Cat(string Name, int Age, bool IsIndoor)
    : Animal(Name, Age);

var dog = new Dog("Rex", 3, "Labrador");
Console.WriteLine(dog);
// Dog { Name = Rex, Age = 3, Breed = Labrador }

Animal animal = dog;
Console.WriteLine(animal is Dog);   // True
Console.WriteLine(animal is Cat);   // False
```

---

# Inheritance and Equality

Equality checks the **runtime type** — a base record does not equal a derived record

```csharp
public record Animal(string Name);
public record Dog(string Name, string Breed) : Animal(Name);

var animal = new Animal("Rex");
var dog    = new Dog("Rex", "Labrador");
var dog2   = new Dog("Rex", "Labrador");

Console.WriteLine(animal == dog);   // False — different types
Console.WriteLine(dog == dog2);     // True  — same type + same data
Console.WriteLine(dog.Equals(animal)); // False
```

<v-clicks>

The generated `Equals` always verifies types match first — no accidental cross-type equality

</v-clicks>

---

# Adding Methods to Records

Records are full types — add any methods, properties, or computed values

```csharp
public record Circle(double Radius)
{
    public double Area        => Math.PI * Radius * Radius;
    public double Circumference => 2 * Math.PI * Radius;

    public bool IsLargerThan(Circle other) => Radius > other.Radius;

    public Circle Scale(double factor) => this with { Radius = Radius * factor };
}

var c1 = new Circle(5);
var c2 = c1.Scale(2);

Console.WriteLine(c1.Area);           // 78.54
Console.WriteLine(c2.Radius);         // 10
Console.WriteLine(c1.IsLargerThan(c2)); // False
```

---

# Adding Validation in the Constructor

Use a **primary constructor body** to add validation

```csharp
public record Temperature(double Celsius)
{
    // Compact constructor — runs AFTER positional assignment
    public Temperature
    {
        if (Celsius < -273.15)
            throw new ArgumentOutOfRangeException(
                nameof(Celsius),
                "Temperature cannot be below absolute zero");
    }

    public double Fahrenheit => Celsius * 9.0 / 5.0 + 32;
    public double Kelvin     => Celsius + 273.15;
}

var t = new Temperature(100);    // OK
var bad = new Temperature(-300); // throws!
```

---

# Compact Constructor Syntax

The compact constructor lets you validate or transform without repeating parameters

```csharp
public record Person(string Name, int Age)
{
    // Compact constructor — no parameter list needed
    public Person
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Name);
        if (Age < 0 || Age > 150)
            throw new ArgumentOutOfRangeException(nameof(Age));

        // Normalize data
        Name = Name.Trim();
    }
}

var p1 = new Person("  Alice  ", 30);
Console.WriteLine(p1.Name); // "Alice" — trimmed
```

---

# Custom Properties Alongside Positional

Mix positional and custom properties

```csharp
public record Product(string Name, decimal Price, int StockCount)
{
    // Computed property
    public bool InStock => StockCount > 0;

    // Property with different access
    public decimal DiscountedPrice => Price * 0.9m;

    // Override a positional property to add validation
    public decimal Price { get; init; } =
        Price <= 0
            ? throw new ArgumentException("Price must be positive")
            : Price;
}

var p = new Product("Widget", 9.99m, 100);
Console.WriteLine(p.InStock);          // True
Console.WriteLine(p.DiscountedPrice);  // 8.991
```

---

# Implementing Interfaces

Records can implement interfaces just like classes

```csharp
public interface IEntity
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
}

public record Order(
    Guid Id,
    DateTime CreatedAt,
    string CustomerName,
    decimal Total
) : IEntity;

IEntity entity = new Order(
    Guid.NewGuid(), DateTime.UtcNow, "Alice", 99.99m);

Console.WriteLine(entity.Id);
Console.WriteLine(entity is Order); // True
```

---

# Deconstruction

Records automatically generate a `Deconstruct` method

```csharp
public record Point(double X, double Y);
public record RGB(byte R, byte G, byte B);

var point = new Point(3.0, 4.0);
var (x, y) = point;
Console.WriteLine($"x={x}, y={y}"); // x=3, y=4

var red = new RGB(255, 0, 0);
var (r, g, b) = red;
Console.WriteLine($"r={r}, g={g}, b={b}"); // r=255, g=0, b=0

// In pattern matching
if (point is (double px, double py) && px > 0)
    Console.WriteLine($"Positive quadrant: {px}, {py}");
```

---

# Pattern Matching with Records

Records work beautifully with `switch` expressions and patterns

```csharp
public abstract record Shape;
public record Circle(double Radius) : Shape;
public record Rectangle(double Width, double Height) : Shape;
public record Triangle(double Base, double Height) : Shape;

double GetArea(Shape shape) => shape switch
{
    Circle   c => Math.PI * c.Radius * c.Radius,
    Rectangle r => r.Width * r.Height,
    Triangle  t => 0.5 * t.Base * t.Height,
    _ => throw new ArgumentException("Unknown shape")
};

Console.WriteLine(GetArea(new Circle(5)));         // 78.54
Console.WriteLine(GetArea(new Rectangle(4, 6)));   // 24
Console.WriteLine(GetArea(new Triangle(3, 8)));    // 12
```

---

# Positional Pattern Matching

Deconstruction enables concise positional patterns

```csharp
public record Point(int X, int Y);

string Classify(Point p) => p switch
{
    (0, 0)       => "Origin",
    (0, _)       => "On Y-axis",
    (_, 0)       => "On X-axis",
    (> 0, > 0)   => "First quadrant",
    (< 0, > 0)   => "Second quadrant",
    (< 0, < 0)   => "Third quadrant",
    _            => "Fourth quadrant"
};

Console.WriteLine(Classify(new Point(0, 0)));    // Origin
Console.WriteLine(Classify(new Point(3, 4)));    // First quadrant
Console.WriteLine(Classify(new Point(-1, 2)));   // Second quadrant
```

---

# Records as DTOs (Data Transfer Objects)

A primary real-world use case — transferring data between layers

```csharp
// API request / response records
public record CreateUserRequest(string Email, string Password, string DisplayName);
public record UserResponse(Guid Id, string Email, string DisplayName, DateTime CreatedAt);
public record ErrorResponse(string Code, string Message);

// In a controller
app.MapPost("/users", (CreateUserRequest req) =>
{
    // validate...
    var user = new UserResponse(
        Id: Guid.NewGuid(),
        Email: req.Email,
        DisplayName: req.DisplayName,
        CreatedAt: DateTime.UtcNow);

    return Results.Created($"/users/{user.Id}", user);
});
```

---

# Records as Domain Events

Records shine for event-driven architectures — immutable, value-equal

```csharp
public abstract record DomainEvent(DateTime OccurredAt);

public record UserRegistered(
    Guid UserId,
    string Email,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);

public record OrderPlaced(
    Guid OrderId,
    Guid CustomerId,
    decimal Total,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);

public record OrderShipped(
    Guid OrderId,
    string TrackingNumber,
    DateTime OccurredAt
) : DomainEvent(OccurredAt);
```

---

# Records as Value Objects (DDD)

Domain-Driven Design value objects — identity defined entirely by data

```csharp
public record Money(decimal Amount, string Currency)
{
    public Money
    {
        if (Amount < 0) throw new ArgumentException("Amount cannot be negative");
        if (string.IsNullOrWhiteSpace(Currency))
            throw new ArgumentException("Currency required");
        Currency = Currency.ToUpperInvariant();
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add different currencies");
        return this with { Amount = Amount + other.Amount };
    }
}

var price   = new Money(9.99m, "usd");
var tax     = new Money(0.80m, "USD");
var total   = price.Add(tax);
Console.WriteLine(total); // Money { Amount = 10.79, Currency = USD }
```

---

# Records and JSON Serialization

Records serialize/deserialize cleanly with `System.Text.Json`

```csharp
using System.Text.Json;

public record Product(string Name, decimal Price, int Quantity);

var product = new Product("Widget", 9.99m, 100);

// Serialize
string json = JsonSerializer.Serialize(product);
Console.WriteLine(json);
// {"Name":"Widget","Price":9.99,"Quantity":100}

// Deserialize
var restored = JsonSerializer.Deserialize<Product>(json);
Console.WriteLine(restored);
// Product { Name = Widget, Price = 9.99, Quantity = 100 }

Console.WriteLine(product == restored); // True — value equality
```

---

# Records and JSON — Custom Names

Use attributes to control JSON property names

```csharp
using System.Text.Json.Serialization;

public record WeatherForecast(
    [property: JsonPropertyName("date")]        DateOnly Date,
    [property: JsonPropertyName("temp_c")]      double TemperatureC,
    [property: JsonPropertyName("summary")]     string? Summary
)
{
    [JsonIgnore]
    public double TemperatureF => TemperatureC * 9 / 5 + 32;
}

var forecast = new WeatherForecast(DateOnly.FromDateTime(DateTime.Today), 22, "Sunny");
Console.WriteLine(JsonSerializer.Serialize(forecast));
// {"date":"2025-06-01","temp_c":22,"summary":"Sunny"}
```

---

# Copying Records in Collections

Build new collections from old ones with `with` — immutable update patterns

```csharp
public record TodoItem(int Id, string Title, bool IsDone);

var todos = new List<TodoItem>
{
    new(1, "Buy groceries", false),
    new(2, "Write tests",   false),
    new(3, "Read book",     false),
};

// Mark item 2 as done — returns a new list
var updated = todos.Select(t =>
    t.Id == 2 ? t with { IsDone = true } : t
).ToList();

updated.ForEach(Console.WriteLine);
// TodoItem { Id = 1, Title = Buy groceries, IsDone = False }
// TodoItem { Id = 2, Title = Write tests,   IsDone = True  }
// TodoItem { Id = 3, Title = Read book,     IsDone = False }
```

---

# readonly record struct

An immutable value-type record — the most restrictive form

```csharp
public readonly record struct Coordinate(double Latitude, double Longitude)
{
    public double DistanceTo(Coordinate other)
    {
        // Haversine approximation (simplified)
        var dlat = other.Latitude  - Latitude;
        var dlon = other.Longitude - Longitude;
        return Math.Sqrt(dlat * dlat + dlon * dlon);
    }
}

var nyc = new Coordinate(40.71, -74.01);
var la  = new Coordinate(34.05, -118.24);

Console.WriteLine(nyc.DistanceTo(la)); // ~48.1 (degrees)
Console.WriteLine(nyc == la);          // False
Console.WriteLine(nyc);                // Coordinate { Latitude = 40.71, Longitude = -74.01 }
```

---

# Sealed Records

Prevent further inheritance with `sealed`

```csharp
public record Animal(string Name);

// This can be inherited
public record Dog(string Name, string Breed) : Animal(Name);

// This cannot be inherited
public sealed record GoldenRetriever(string Name) : Dog(Name, "Golden Retriever");

// public record MiniGolden(...) : GoldenRetriever(...); // ERROR
```

<v-clicks>

`sealed` on a record also allows the compiler to de-virtualize some equality calls — minor perf win

</v-clicks>

---

# Abstract Records

Use `abstract record` as a base type — cannot be instantiated directly

```csharp
public abstract record Notification(string Title, DateTime SentAt);

public record EmailNotification(
    string Title, DateTime SentAt, string ToAddress) : Notification(Title, SentAt);

public record PushNotification(
    string Title, DateTime SentAt, string DeviceToken) : Notification(Title, SentAt);

void Send(Notification n)
{
    Console.WriteLine(n switch
    {
        EmailNotification e => $"Email to {e.ToAddress}: {e.Title}",
        PushNotification  p => $"Push to {p.DeviceToken}: {p.Title}",
        _                   => "Unknown notification"
    });
}
```

---

# Records and GetHashCode

Hash code is based on **all properties** — safe for dictionary keys

```csharp
public record Point(int X, int Y);

var dict = new Dictionary<Point, string>
{
    [new Point(0, 0)] = "Origin",
    [new Point(1, 0)] = "Right",
    [new Point(0, 1)] = "Up",
};

// Lookup by value — works because records implement GetHashCode correctly
Console.WriteLine(dict[new Point(0, 0)]); // Origin
Console.WriteLine(dict[new Point(1, 0)]); // Right

// Two equal records always produce the same hash code
var p1 = new Point(3, 4);
var p2 = new Point(3, 4);
Console.WriteLine(p1.GetHashCode() == p2.GetHashCode()); // True
```

---

# Overriding Generated Members

You can override any of the generated methods

```csharp
public record Product(string Name, decimal Price)
{
    // Custom ToString — override the generated one
    public override string ToString() =>
        $"{Name} @ ${Price:F2}";

    // Override Equals to ignore Price in comparison
    public virtual bool Equals(Product? other) =>
        other is not null && Name == other.Name;

    public override int GetHashCode() => HashCode.Combine(Name);
}

var p1 = new Product("Widget", 9.99m);
var p2 = new Product("Widget", 14.99m);

Console.WriteLine(p1 == p2);   // True — name-only equality
Console.WriteLine(p1);         // Widget @ $9.99
```

---

# Records vs Classes vs Structs

| | class | struct | record class | record struct |
|--|-------|--------|--------------|---------------|
| Reference type | ✅ | ❌ | ✅ | ❌ |
| Value equality | Manual | Auto | Auto | Auto |
| Immutable by default | ❌ | ❌ | ✅ | ✅ |
| `with` expression | ❌ | ❌ | ✅ | ✅ |
| `ToString` | Manual | Manual | Auto | Auto |
| Inheritance | ✅ | ❌ | ✅ | ❌ |
| Null allowed | ✅ | `?` only | ✅ | `?` only |
| Best for | Behavior/identity | Small perf values | Data carriers | Tiny immutable data |

---

# When to Use Records

<v-clicks>

**Use `record class` when:**
- Representing immutable data: DTOs, API responses, config
- Building value objects in DDD
- Modeling domain events or commands
- You want value equality without boilerplate

**Use `record struct` when:**
- Data is small (fits guideline of ≤ 16 bytes)
- You want value semantics + no heap allocation
- Coordinates, colors, ranges, small measurements

**Stick with `class` when:**
- Object has mutable state and behavior
- Object has identity beyond its data (e.g., a service, connection)
- You need complex inheritance hierarchies

</v-clicks>

---

# Common Pitfalls

```csharp
// 1. Mutable reference-type properties break value equality
public record Broken(List<int> Items);
var a = new Broken(new List<int> { 1, 2 });
var b = new Broken(new List<int> { 1, 2 });
Console.WriteLine(a == b); // False! List uses reference equality

// Fix: use immutable collections
public record Fixed(ImmutableArray<int> Items);

// 2. with does a shallow copy — nested classes are shared
public record Outer(Inner Child);
public class Inner { public int Value { get; set; } }
var x = new Outer(new Inner { Value = 1 });
var y = x with { };           // shallow copy
y.Child.Value = 99;
Console.WriteLine(x.Child.Value); // 99 — x is affected!
```

---

# Records with Generics

Records support generic type parameters

```csharp
public record Result<T>(bool IsSuccess, T? Value, string? Error)
{
    public static Result<T> Ok(T value) =>
        new(IsSuccess: true, Value: value, Error: null);

    public static Result<T> Fail(string error) =>
        new(IsSuccess: false, Value: default, Error: error);
}

var success = Result<int>.Ok(42);
var failure = Result<int>.Fail("Not found");

Console.WriteLine(success); // Result { IsSuccess = True, Value = 42, Error =  }
Console.WriteLine(failure); // Result { IsSuccess = False, Value = 0, Error = Not found }

if (success.IsSuccess)
    Console.WriteLine($"Got: {success.Value}"); // Got: 42
```

---

# Best Practices

<v-clicks>

- **Prefer positional records** for simple data — one-liner, clear, concise
- **Use compact constructor** for validation — don't skip it for important invariants
- **Keep records small and focused** — if a type has lots of behavior, use a class
- **Avoid mutable properties** in records unless you have a strong reason
- **Use `ImmutableArray<T>`** or `IReadOnlyList<T>` instead of `List<T>` in records to preserve value equality
- **Leverage pattern matching** — records and `switch` expressions are a perfect pair
- **`record struct`** for frequently-allocated, small, pure-data types
- **`sealed record`** when you know a record won't be subclassed — cleaner and slightly faster
- **Don't override `Equals`** unless you have a specific reason — the default is usually correct

</v-clicks>

---

# Summary

Records are C#'s answer to **data-oriented programming**

<v-clicks>

- `record` / `record class` — immutable reference type with value equality
- `record struct` — immutable value type with value equality
- `readonly record struct` — adds compile-time immutability enforcement
- Automatically generates: `Equals`, `==`, `GetHashCode`, `ToString`, `Deconstruct`
- `with` expressions allow safe, non-destructive "mutations"
- Support inheritance, interfaces, generics, and custom methods
- Excellent fit for: DTOs, domain events, value objects, API models, config

</v-clicks>

---

# Questions?

## Topics We Covered

- What records are and the problem they solve
- Positional syntax and generated members
- Value-based equality and `GetHashCode`
- `with` expressions — non-destructive mutation
- `record class` vs `record struct` vs `readonly record struct`
- Inheritance, abstract, and sealed records
- Validation in compact constructors
- Pattern matching and deconstruction
- Records as DTOs, domain events, and value objects
- JSON serialization
- Generics with records
- Common pitfalls and best practices
