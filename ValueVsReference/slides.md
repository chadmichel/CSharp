---
theme: apple-basic
colorSchema: light
info: |
  ## C# Value vs Reference Types
  A deep dive into how C# manages memory with value and reference types.
drawings:
  persist: false
transition: slide-left
title: C# Value vs Reference Types
mdc: true
---

<style src="./style.css"></style>

# C# Value vs Reference Types

Understanding how C# stores, copies, and manages data in memory

<div class="pt-12">
  <span @click="$slidev.nav.next" class="px-2 py-1 rounded cursor-pointer" hover="bg-white bg-opacity-10">
    Press Space for next page <carbon:arrow-right class="inline"/>
  </span>
</div>

---

# The Big Picture

Every variable in C# is either a **value type** or a **reference type**

<v-clicks>

- **Value types** store data directly — the variable *is* the data
- **Reference types** store a reference (pointer) — the variable *points to* the data
- This distinction affects: copying, equality, memory, performance, and API design
- Understanding this is foundational to writing correct C# code

</v-clicks>

---

# Value Types — The Basics

A value type variable holds its data directly in its own memory location

```csharp
int a = 10;
int b = a;   // b gets its own copy of 10

b = 99;      // changing b does NOT affect a

Console.WriteLine(a); // 10
Console.WriteLine(b); // 99
```

<v-clicks>

- `b` is an **independent copy** of `a`
- Modifying one does not affect the other
- Each variable owns its data

</v-clicks>

---

# Reference Types — The Basics

A reference type variable holds a *reference* (address) to data on the heap

```csharp
var list1 = new List<int> { 1, 2, 3 };
var list2 = list1;   // list2 points to the SAME list

list2.Add(99);

Console.WriteLine(list1.Count); // 4 — list1 sees the change!
Console.WriteLine(list2.Count); // 4
```

<v-clicks>

- `list2` and `list1` both **point to the same object**
- Modifying through one reference is visible through the other
- No data was copied — only the address was copied

</v-clicks>

---

# Stack vs Heap

C# uses two memory regions to store data

<v-clicks>

### Stack
- **Fast** allocation and deallocation (just move a pointer)
- **Fixed size** — known at compile time
- **LIFO** — last in, first out
- Value types typically live here (when local variables)

### Heap
- **Flexible** — can grow and shrink
- Managed by the **Garbage Collector**
- Reference type objects always live here
- Slightly more overhead than stack

</v-clicks>

---

# Stack vs Heap Diagram

```
Stack                    Heap
─────────────────        ──────────────────────────────
int x = 5;
│ x = 5         │
│               │
var p = new     ─────►  │ Person { Name="Alice" }     │
    Person();           │                              │
│ p = [addr]    │       │                              │
─────────────────        ──────────────────────────────
```

<v-clicks>

- `x` (int) lives entirely on the stack
- `p` (Person reference) lives on the stack, but the **object** lives on the heap
- The stack holds the *address*, not the *object*

</v-clicks>

---

# Built-in Value Types

C# has many built-in value types

```csharp
// Integer types
byte b = 255;
short s = 32767;
int i = 2_147_483_647;
long l = 9_223_372_036_854_775_807L;

// Floating point
float f = 3.14f;
double d = 3.141592653589793;
decimal m = 9.99m;   // precise — great for money

// Other
bool flag = true;
char ch = 'A';

// Struct-based
DateTime now = DateTime.Now;
Guid id = Guid.NewGuid();
```

---

# Built-in Reference Types

```csharp
// string — reference type (but behaves like value type!)
string name = "Alice";

// Arrays — always reference types
int[] numbers = { 1, 2, 3 };

// Classes
var person = new Person();
var list   = new List<int>();
var dict   = new Dictionary<string, int>();

// object — the base of all types
object obj = 42;

// dynamic
dynamic dyn = "hello";
```

---

# Defining Your Own Value Type: struct

Use `struct` to create a custom value type

```csharp
public struct Point
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double DistanceTo(Point other)
    {
        double dx = X - other.X;
        double dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
```

```csharp
var p1 = new Point(0, 0);
var p2 = new Point(3, 4);
Console.WriteLine(p1.DistanceTo(p2)); // 5
```

---

# Defining Your Own Reference Type: class

Use `class` to create a custom reference type

```csharp
public class BankAccount
{
    public string Owner { get; set; }
    public decimal Balance { get; private set; }

    public BankAccount(string owner, decimal initialBalance)
    {
        Owner = owner;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount) => Balance += amount;
    public void Withdraw(decimal amount) => Balance -= amount;
}
```

```csharp
var acct1 = new BankAccount("Alice", 1000m);
var acct2 = acct1;   // same object!

acct2.Deposit(500m);
Console.WriteLine(acct1.Balance); // 1500 — both see it
```

---

# Struct vs Class — Quick Comparison

| Feature | struct | class |
|---------|--------|-------|
| Memory location | Stack (usually) | Heap |
| Copy behavior | Full copy | Reference copy |
| Default value | Zero/empty | null |
| Inheritance | No (only interfaces) | Yes |
| Nullable without `?` | No | Yes |
| Performance | Better for small types | Better for large types |
| Identity | No (value equality) | Yes (reference identity) |

---

# Copying Value Types

Each copy is fully independent

```csharp
public struct Rectangle
{
    public int Width;
    public int Height;
}

var r1 = new Rectangle { Width = 10, Height = 5 };
var r2 = r1;   // full copy of all fields

r2.Width = 99; // only r2 changes

Console.WriteLine(r1.Width); // 10 — unchanged
Console.WriteLine(r2.Width); // 99
```

<v-clicks>

**All fields are copied** — even nested value types within a struct are copied

</v-clicks>

---

# Copying Reference Types

Only the reference is copied — both point to the same object

```csharp
public class Rectangle
{
    public int Width;
    public int Height;
}

var r1 = new Rectangle { Width = 10, Height = 5 };
var r2 = r1;   // r2 points to the SAME object

r2.Width = 99; // changes the shared object

Console.WriteLine(r1.Width); // 99 — r1 sees it too!
Console.WriteLine(r2.Width); // 99
```

<v-clicks>

To get an independent copy of a class, you must implement **cloning** explicitly

</v-clicks>

---

# Deep Copy of a Reference Type

```csharp
public class Person
{
    public string Name { get; set; }
    public Address HomeAddress { get; set; }

    // Manual deep copy
    public Person Clone() => new Person
    {
        Name = this.Name,
        HomeAddress = new Address
        {
            Street = this.HomeAddress.Street,
            City   = this.HomeAddress.City
        }
    };
}

var alice = new Person { Name = "Alice", HomeAddress = new Address { City = "NYC" } };
var bob   = alice.Clone(); // truly independent copy

bob.HomeAddress.City = "LA";
Console.WriteLine(alice.HomeAddress.City); // NYC — unaffected
```

---

# Equality: Value Types

Value types compare **by value** — two variables are equal if their data matches

```csharp
int x = 5;
int y = 5;
Console.WriteLine(x == y);  // True — same value

var p1 = new Point(1, 2);
var p2 = new Point(1, 2);
Console.WriteLine(p1 == p2);  // True for struct (by default compares fields)
```

<v-clicks>

Custom structs get **value-based equality** by default — two structs are equal if all fields match

</v-clicks>

---

# Equality: Reference Types

Reference types compare **by reference** (identity) by default

```csharp
var a = new List<int> { 1, 2, 3 };
var b = new List<int> { 1, 2, 3 };

Console.WriteLine(a == b);           // False — different objects!
Console.WriteLine(ReferenceEquals(a, b)); // False

var c = a;
Console.WriteLine(a == c);           // True — same object
Console.WriteLine(ReferenceEquals(a, c)); // True
```

<v-clicks>

Override `Equals()` and `GetHashCode()` to get value-based equality for classes

</v-clicks>

---

# Overriding Equality for a Class

```csharp
public class Point
{
    public int X { get; init; }
    public int Y { get; init; }

    public override bool Equals(object? obj) =>
        obj is Point other && X == other.X && Y == other.Y;

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(Point? a, Point? b) => Equals(a, b);
    public static bool operator !=(Point? a, Point? b) => !Equals(a, b);
}

var p1 = new Point { X = 1, Y = 2 };
var p2 = new Point { X = 1, Y = 2 };
Console.WriteLine(p1 == p2);  // True — value equality
```

---

# record and record struct (C# 9+)

`record` gives you value-based equality automatically

```csharp
// record class — reference type with value equality
public record PersonRecord(string Name, int Age);

var r1 = new PersonRecord("Alice", 30);
var r2 = new PersonRecord("Alice", 30);
Console.WriteLine(r1 == r2);  // True — value equality!

// record struct — value type with value equality
public record struct PointRecord(double X, double Y);

var p1 = new PointRecord(1.0, 2.0);
var p2 = new PointRecord(1.0, 2.0);
Console.WriteLine(p1 == p2);  // True
```

<v-clicks>

Records also generate `ToString()`, `with` expressions, and deconstruction for free

</v-clicks>

---

# with Expressions (Records)

Non-destructive mutation — create a copy with some changes

```csharp
public record Person(string Name, int Age, string City);

var alice = new Person("Alice", 30, "NYC");

// Create a modified copy — original is unchanged
var olderAlice = alice with { Age = 31 };
var movedAlice = alice with { City = "LA" };

Console.WriteLine(alice);       // Person { Name = Alice, Age = 30, City = NYC }
Console.WriteLine(olderAlice);  // Person { Name = Alice, Age = 31, City = NYC }
Console.WriteLine(movedAlice);  // Person { Name = Alice, Age = 30, City = LA }
```

---

# Passing Arguments: Value Types

By default, value types are **passed by value** — a copy is made

```csharp
void Double(int x)
{
    x = x * 2;   // modifies the LOCAL copy
}

int n = 10;
Double(n);
Console.WriteLine(n); // still 10 — original unchanged
```

<v-clicks>

The method receives its own copy — callers are protected from unexpected mutations

</v-clicks>

---

# Passing Arguments: Reference Types

Reference types pass a copy of the **reference** — the object is shared

```csharp
void AddItem(List<int> list)
{
    list.Add(99);   // modifies the shared object
}

var nums = new List<int> { 1, 2, 3 };
AddItem(nums);
Console.WriteLine(nums.Count); // 4 — modified!
```

<v-clicks>

But reassigning the parameter only affects the local copy of the reference:

```csharp
void Replace(List<int> list)
{
    list = new List<int> { 99 }; // only local ref changes
}
Replace(nums); // nums is unchanged
```

</v-clicks>

---

# The ref Keyword

`ref` passes a value type **by reference** — the method operates on the original

```csharp
void Double(ref int x)
{
    x = x * 2;  // modifies the ORIGINAL
}

int n = 10;
Double(ref n);
Console.WriteLine(n); // 20 — original was changed!
```

```csharp
// Also works with structs
void MoveRight(ref Point p, double dx)
{
    p.X += dx;
}

var point = new Point(0, 0);
MoveRight(ref point, 5.0);
Console.WriteLine(point.X); // 5.0
```

---

# The out Keyword

`out` is like `ref` but the method **must assign** the value

```csharp
bool TryParse(string input, out int result)
{
    if (int.TryParse(input, out result))
        return true;

    result = 0;  // must assign before returning
    return false;
}

if (TryParse("42", out int value))
    Console.WriteLine($"Parsed: {value}"); // Parsed: 42

if (!TryParse("abc", out int bad))
    Console.WriteLine("Failed to parse");
```

<v-clicks>

The `Try*` pattern — return bool + populate result via `out` — is idiomatic C#

</v-clicks>

---

# The in Keyword

`in` passes by reference but **prevents modification** — read-only reference

```csharp
// Large struct — passing by value copies all fields
public struct BigMatrix
{
    public double[,] Data; // hypothetically large
}

// Use 'in' to avoid copying without allowing mutation
double ComputeSum(in BigMatrix m)
{
    double sum = 0;
    // m.Data = null;  // ERROR — cannot modify 'in' parameter
    foreach (var v in m.Data) sum += v;
    return sum;
}
```

<v-clicks>

`in` is a performance optimization for large structs — avoids copying while ensuring safety

</v-clicks>

---

# Default Values

What is a variable's value before you assign it?

```csharp
int i;      // default: 0
bool b;     // default: false
double d;   // default: 0.0
char c;     // default: '\0'
DateTime dt; // default: DateTime.MinValue (0001-01-01)

string s;   // default: null  (reference type)
object o;   // default: null
int[] arr;  // default: null
```

```csharp
// default keyword — explicit default value
int x = default;           // 0
Point p = default;         // Point { X=0, Y=0 }
string s = default;        // null
List<int> list = default;  // null
```

---

# Nullable Value Types

Value types can't normally be `null` — use `?` to allow it

```csharp
int  normal = null;   // COMPILE ERROR
int? nullable = null; // OK!

int? age = null;

if (age.HasValue)
    Console.WriteLine($"Age: {age.Value}");
else
    Console.WriteLine("Age unknown");

// Null-coalescing operator
int displayAge = age ?? 0;

// Null-conditional (safe access)
int? doubled = age * 2;  // null if age is null
```

<v-clicks>

`int?` is shorthand for `Nullable<int>` — a struct that wraps the value and a bool flag

</v-clicks>

---

# Nullable Reference Types (C# 8+)

Enable nullable annotations to catch `null` dereference bugs at compile time

```csharp
#nullable enable

string name = "Alice";   // cannot be null
string? nickname = null; // explicitly nullable

void Greet(string? name)
{
    // compiler warns if you dereference without null check
    Console.WriteLine(name.Length); // WARNING!

    if (name != null)
        Console.WriteLine(name.Length); // OK

    Console.WriteLine(name?.Length ?? 0); // OK — null-safe
}
```

<v-clicks>

Enable project-wide in `.csproj`: `<Nullable>enable</Nullable>`

</v-clicks>

---

# Boxing: Value Type → object

**Boxing** wraps a value type in a heap-allocated object

```csharp
int number = 42;
object boxed = number;   // BOXING — copies int to the heap

Console.WriteLine(boxed);        // 42
Console.WriteLine(boxed.GetType()); // System.Int32
```

```csharp
// Boxing happens implicitly in many situations:
ArrayList list = new ArrayList();
list.Add(42);   // boxes the int!
list.Add(true); // boxes the bool!

// Use generic collections to avoid boxing:
List<int> safeList = new List<int>();
safeList.Add(42); // NO boxing
```

---

# Unboxing: object → Value Type

**Unboxing** extracts the value back — requires an explicit cast

```csharp
object boxed = 42;         // boxed int
int unboxed = (int)boxed;  // unboxing — explicit cast required

Console.WriteLine(unboxed); // 42
```

```csharp
// Wrong type → InvalidCastException at runtime!
object boxed = 42;
double d = (double)boxed;  // throws InvalidCastException!
// Must match the original type exactly:
double d2 = (double)(int)boxed; // OK — unbox to int, then convert
```

<v-clicks>

Boxing/unboxing has a **performance cost**: heap allocation + GC pressure. Avoid in hot paths.

</v-clicks>

---

# Boxing Performance Impact

```csharp
// BAD: boxing on every iteration
var list = new ArrayList();
for (int i = 0; i < 1_000_000; i++)
    list.Add(i);   // 1 million heap allocations!

// GOOD: no boxing with generics
var goodList = new List<int>();
for (int i = 0; i < 1_000_000; i++)
    goodList.Add(i);  // stored directly — no heap allocation
```

```csharp
// Also watch out for interface calls on structs:
IComparable c = 42; // boxes the int to call interface method
```

<v-clicks>

Generics were introduced in C# 2.0 specifically to eliminate boxing in collections

</v-clicks>

---

# readonly struct

Declare a struct as immutable — compiler enforces it

```csharp
public readonly struct Temperature
{
    public double Celsius { get; }
    public double Fahrenheit => Celsius * 9.0 / 5.0 + 32;

    public Temperature(double celsius)
    {
        Celsius = celsius;
    }

    public Temperature AddDegrees(double amount) =>
        new Temperature(Celsius + amount); // returns new instance
}

var t1 = new Temperature(20);
var t2 = t1.AddDegrees(5);

Console.WriteLine(t1.Celsius); // 20 — unchanged
Console.WriteLine(t2.Celsius); // 25
```

---

# ref struct

A struct that **must** live on the stack — cannot be boxed or stored on heap

```csharp
// ref struct cannot be boxed, stored in a field,
// used as a type argument, or captured in a lambda
public ref struct StackOnlyBuffer
{
    private Span<byte> _buffer;

    public StackOnlyBuffer(Span<byte> buffer)
    {
        _buffer = buffer;
    }

    public void Fill(byte value) => _buffer.Fill(value);
}
```

<v-clicks>

`ref struct` is primarily used for high-performance, allocation-free code — e.g., `Span<T>`, `ReadOnlySpan<T>`

</v-clicks>

---

# Span&lt;T&gt; — Stack-Based Slice

`Span<T>` is a `ref struct` that provides a **window** into contiguous memory

```csharp
int[] array = { 1, 2, 3, 4, 5, 6, 7, 8 };

// Slice without allocation — just a pointer + length
Span<int> slice = array.AsSpan(2, 4); // { 3, 4, 5, 6 }

foreach (var item in slice)
    Console.Write($"{item} "); // 3 4 5 6

// Modify through the span — affects original array
slice[0] = 99;
Console.WriteLine(array[2]); // 99
```

```csharp
// Works with stack-allocated memory too!
Span<int> stackMem = stackalloc int[8];
stackMem.Fill(0);
```

---

# Strings — A Special Reference Type

`string` is a reference type, but it **behaves** like a value type

```csharp
string a = "hello";
string b = a;

b = "world"; // b now points to a new string — a is unaffected

Console.WriteLine(a); // hello
Console.WriteLine(b); // world
```

<v-clicks>

Strings are **immutable** — any "modification" creates a new string object

```csharp
string s = "hello";
s += " world";  // creates a NEW string; original "hello" is unchanged
```

Use `StringBuilder` when building strings in a loop to avoid excessive allocations

</v-clicks>

---

# String Interning

The runtime may **reuse** string literals to save memory

```csharp
string a = "hello";
string b = "hello";

// Might be the same reference due to interning!
Console.WriteLine(ReferenceEquals(a, b)); // True (interned literals)

string c = new string('h', 1) + "ello"; // dynamic creation
Console.WriteLine(ReferenceEquals(a, c)); // False (not interned)

// Force intern
string d = string.Intern(c);
Console.WriteLine(ReferenceEquals(a, d)); // True
```

<v-clicks>

Always compare strings with `==` or `.Equals()` — never rely on `ReferenceEquals` for string equality

</v-clicks>

---

# Interfaces with Value Types

Value types can implement interfaces — but watch out for boxing

```csharp
public interface IArea
{
    double GetArea();
}

public struct Circle : IArea
{
    public double Radius { get; init; }
    public double GetArea() => Math.PI * Radius * Radius;
}

Circle c = new Circle { Radius = 5 };
double area = c.GetArea(); // NO boxing — called directly on struct

// Boxing happens when assigned to interface variable:
IArea shape = c;           // BOXING — Circle copied to heap
double area2 = shape.GetArea(); // called on boxed copy
```

---

# Collections: Value vs Reference Elements

How element type affects collection behavior

```csharp
// Value type elements — each element IS the data
var points = new List<Point> { new(1,1), new(2,2) };
var p = points[0];
p.X = 99;                   // modifies local copy only!
Console.WriteLine(points[0].X); // still 1

// Reference type elements — each element is a reference
var people = new List<Person> { new() { Name = "Alice" } };
var person = people[0];
person.Name = "Bob";        // modifies the shared object
Console.WriteLine(people[0].Name); // "Bob" — changed!
```

<v-clicks>

**Common pitfall**: modifying a value-type element retrieved from a list has no effect on the list

</v-clicks>

---

# Arrays: Value vs Reference Types

```csharp
// Array of value types — contiguous memory block
int[] ints = new int[4]; // one contiguous allocation
// Memory: [0][0][0][0] — all data inline

// Array of reference types — array of pointers
string[] strings = new string[4];
// Memory: [null][null][null][null] — each points to heap (or null)

strings[0] = "hello"; // heap: ["hello"] ←── strings[0]
```

```csharp
// Copying an array
int[]    srcInts    = { 1, 2, 3 };
int[]    dstInts    = (int[])srcInts.Clone();    // deep copy — values
Person[] srcPeople  = { new() { Name = "A" } };
Person[] dstPeople  = (Person[])srcPeople.Clone(); // shallow — same objects!
```

---

# Common Pitfall: Mutable Struct in a Field

Mutating a struct field through a property creates a hidden copy

```csharp
public class Game
{
    public Point PlayerPosition { get; set; } // struct property

    public void MovePlayer()
    {
        // WRONG: modifies a copy, not the actual field!
        // PlayerPosition.X += 1; // compiler error in newer C#

        // CORRECT: replace the entire value
        PlayerPosition = new Point(PlayerPosition.X + 1, PlayerPosition.Y);
    }
}
```

<v-clicks>

This is why **immutable structs** (or `readonly struct`) are safer — mutations are always explicit

</v-clicks>

---

# Common Pitfall: Closure Over a Value Type

Lambdas capture a **reference** to the variable, not a copy of the value

```csharp
// Loop variable capture
var actions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    actions.Add(() => Console.WriteLine(i)); // captures 'i' by reference
}
actions.ForEach(a => a()); // prints 3, 3, 3 — not 0, 1, 2!

// Fix: capture a local copy
for (int i = 0; i < 3; i++)
{
    int captured = i;
    actions.Add(() => Console.WriteLine(captured)); // 0, 1, 2
}
```

---

# Common Pitfall: null and Value Types

```csharp
// This compiles but throws at runtime:
object obj = null;
int n = (int)obj;   // NullReferenceException!

// Safer unboxing:
int? safe = obj as int?;
if (safe.HasValue)
    Console.WriteLine(safe.Value);

// Pattern matching (modern approach):
if (obj is int value)
    Console.WriteLine(value);
```

<v-clicks>

`as` returns `null` on failure for reference types, but `as int` is not valid — use `int?` or `is`

</v-clicks>

---

# struct vs class — When to Use Each

### Prefer **struct** when:
- The type is small (16 bytes or less is a common guideline)
- It's logically a **value** — like a coordinate, color, or range
- It will be short-lived and frequently allocated
- You want **immutability** and value semantics
- Examples: `Point`, `Color`, `DateTime`, `Guid`

### Prefer **class** when:
- The type has **identity** — two instances can represent distinct things even with the same data
- It's large or has many fields
- You need **inheritance**
- It holds mutable state that should be shared
- Examples: `BankAccount`, `HttpClient`, `Stream`

---

# Performance: Stack Allocation

Struct allocation on the stack is essentially free — just move the stack pointer

```csharp
// Stack allocated — extremely fast
void ProcessPoint()
{
    var p = new Point(1.0, 2.0); // stack allocation
    // ... use p ...
} // p is freed instantly when method returns

// Heap allocated — involves GC
void ProcessPerson()
{
    var person = new Person("Alice"); // heap allocation
    // ... use person ...
} // person is eligible for GC — not immediately freed
```

<v-clicks>

For very hot code paths, prefer structs to reduce GC pressure

</v-clicks>

---

# Performance: Large Structs

Copying large structs can be **slower** than passing class references

```csharp
// Large struct — expensive to copy
public struct LargeData
{
    public double A, B, C, D, E, F, G, H; // 64 bytes
}

// Each call copies all 64 bytes
void Process(LargeData data) { ... }

// Use 'in' to pass by reference without copying:
void ProcessFast(in LargeData data) { ... }

// Or use a class — only the reference (8 bytes) is copied:
public class LargeDataClass
{
    public double A, B, C, D, E, F, G, H;
}
```

---

# Value Types and Thread Safety

Value types are **not inherently thread-safe**, but their copy semantics help

```csharp
// Value type — each thread gets its own copy (if passed by value)
void ThreadSafeWork(int localValue)
{
    // modifying localValue is safe — it's a copy
    localValue *= 2;
}

// Reference type — shared state needs synchronization
private readonly List<int> _shared = new();
private readonly Lock _lock = new();

void AddItem(int item)
{
    lock (_lock) { _shared.Add(item); }
}
```

<v-clicks>

Shared mutable state (whether value or reference type) always needs synchronization

</v-clicks>

---

# Pattern Matching with Types

`is` and `switch` work beautifully with value and reference types

```csharp
void Describe(object obj)
{
    switch (obj)
    {
        case int i when i < 0:
            Console.WriteLine($"Negative int: {i}");
            break;
        case int i:
            Console.WriteLine($"Positive int: {i}");
            break;
        case string s:
            Console.WriteLine($"String: {s}");
            break;
        case null:
            Console.WriteLine("null");
            break;
        default:
            Console.WriteLine($"Other: {obj.GetType().Name}");
            break;
    }
}
```

---

# Type Checking

```csharp
object value = 42;

// is — returns bool
if (value is int)
    Console.WriteLine("It's an int");

// is with declaration (pattern variable)
if (value is int n)
    Console.WriteLine($"Int value: {n}");

// GetType() — exact runtime type
Console.WriteLine(value.GetType());         // System.Int32
Console.WriteLine(value.GetType().Name);    // Int32

// typeof — compile-time type
Console.WriteLine(typeof(int));             // System.Int32
Console.WriteLine(value.GetType() == typeof(int)); // True
```

---

# Summary: Key Differences

| | Value Type | Reference Type |
|--|------------|---------------|
| Storage | Stack (typically) | Heap |
| Copy | Full independent copy | Copy of reference |
| Default | `0` / `false` / `\0` | `null` |
| Null | Only with `?` | Yes (unless `#nullable enable`) |
| Equality | By value (fields) | By reference (identity) |
| Inheritance | No | Yes |
| Boxing | When cast to `object` | Never |
| GC | No (stack-based) | Yes |
| Keyword | `struct` | `class` |

---

# Best Practices

<v-clicks>

- **Prefer immutable structs** — use `readonly struct` and `init` properties
- **Use `record` / `record struct`** for data carriers — free equality, `ToString`, and `with`
- **Avoid large mutable structs** — copying is expensive and behavior is surprising
- **Use generics** (`List<T>` not `ArrayList`) to avoid boxing
- **Pass large structs with `in`** to avoid copies in hot paths
- **Use `Nullable<T>` / `T?`** to express optional value types explicitly
- **Enable `#nullable enable`** to catch null reference bugs at compile time
- **Understand closure capture** — closures capture variables, not values
- **Use `StringBuilder`** for string concatenation in loops

</v-clicks>

---

# Questions?

## Topics We Covered

- Value types vs reference types fundamentals
- Stack vs heap memory
- `struct` vs `class` definition and behavior
- Copy semantics and equality
- `ref`, `out`, `in` parameter modifiers
- Boxing and unboxing
- `record` and `record struct`
- `readonly struct` and `ref struct`
- `Span<T>` and stack memory
- Nullable types (`T?`, `#nullable enable`)
- Common pitfalls and best practices
