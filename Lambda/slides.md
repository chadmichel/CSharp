---
theme: apple-basic
colorSchema: light
info: |
  ## C# Lambdas
  A concise, practical guide to lambda expressions in C#.
drawings:
  persist: false
transition: slide-left
title: C# Lambdas
mdc: true
---

<style src="./style.css"></style>

# C# Lambdas

Inline, anonymous functions for concise, expressive code

<div class="pt-12">
  <span @click="$slidev.nav.next" class="px-2 py-1 rounded cursor-pointer" hover="bg-white bg-opacity-10">
    Press Space for next page <carbon:arrow-right class="inline"/>
  </span>
</div>
---

# Overview

Lambdas are anonymous functions you can pass around like values.

```csharp
// (parameters) => expression
// (parameters) => { statements; return value; }
Func<int, int> doubleIt = x => x * 2;
Action<string> log = m => Console.WriteLine(m);
```

---

# Action vs Func

`Action` delegates return void; `Func` delegates return a value.

```csharp
// Action: no return value
Action sayHi = () => Console.WriteLine("Hi");
Action<string> logMsg = msg => Console.WriteLine(msg);
Action<int, int> addLog = (a, b) => Console.WriteLine(a + b);

// Func: last generic type is the return type
Func<int> getRandom = () => Random.Shared.Next();
Func<int, int> square = x => x * x;
Func<int, int, int> add = (a, b) => a + b;

// Predicate<T> is a shorthand for Func<T, bool>
Predicate<string> nonEmpty = s => !string.IsNullOrWhiteSpace(s);
```

Guidelines:
- Use `Action` for side effects
- Use `Func` when you need a result
- Use `Predicate<T>` for conditions; `Comparison<T>` for sorting

---

# How To Call / Use

Assign lambdas to variables or pass them directly to methods.

```csharp
// Assign to a variable
Func<int, bool> isOdd = n => (n & 1) == 1;
Console.WriteLine(isOdd(3)); // True

// Pass inline
var top3 = scores
  .OrderByDescending(s => s)
  .Take(3)
  .ToList();

// With events
button.Click += (s, e) => Console.WriteLine("Clicked!");

// Closures (captures external variable)
var threshold = 10;
var big = nums.Where(n => n > threshold).ToList();
threshold = 5; // capture is by reference; later changes affect calls
```

Tips:
- Prefer expression bodies for short logic
- Use explicit types for readability when needed
- Be mindful of captured variables (especially in loops)

---

# Why They Exist

Lambdas make code shorter, clearer, and more composable.

- Express callbacks and small functions inline (no separate method)
- Power functional-style APIs (LINQ: `Select`, `Where`, etc.)
- Enable event handlers and async continuations
- Capture context via closures for flexible behavior
- Back expression trees for providers like Entity Framework

```csharp
var evens = numbers.Where(n => n % 2 == 0).ToList();
var names = people.Select(p => p.Name).ToArray();
```
