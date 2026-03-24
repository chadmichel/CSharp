---
theme: apple-basic
colorSchema: light
info: |
  ## Testing with MSTest and C#
  A practical guide to unit testing in C# using the MSTest framework.
drawings:
  persist: false
transition: slide-left
title: Testing with MSTest and C#
mdc: true
---

<style src="./style.css"></style>

# Testing with MSTest and C#

Writing reliable, maintainable unit tests in C#

<div class="pt-12">
  <span @click="$slidev.nav.next" class="px-2 py-1 rounded cursor-pointer" hover="bg-white bg-opacity-10">
    Press Space for next page <carbon:arrow-right class="inline"/>
  </span>
</div>

---

# Why Write Tests?

<v-clicks>

- **Catch bugs early** — find regressions before they reach production
- **Document behavior** — tests describe what the code is *supposed* to do
- **Enable refactoring** — change code confidently when tests have your back
- **Faster feedback** — run tests in seconds instead of manually clicking through UIs
- **Better design** — testable code tends to be better structured (loosely coupled)
- **Sleep at night** — confidence that the system works as expected

</v-clicks>

---

# Types of Tests

<v-clicks>

### Unit Tests
Test a single class or function in **isolation** — fast, focused, no real I/O

### Integration Tests
Test how multiple components work **together** — may hit a database or API

### End-to-End Tests
Test the full application through the UI — slow, but highest confidence

</v-clicks>

<v-clicks>

We'll focus on **unit tests** — they're the foundation.

The "Testing Pyramid": many unit tests, fewer integration tests, fewest E2E tests.

</v-clicks>

---

# MSTest Overview

MSTest is Microsoft's built-in testing framework for .NET

<v-clicks>

- Ships with Visual Studio
- Runs in the `dotnet test` CLI
- Works in VS Code, Rider, and CI/CD pipelines
- Attributes drive test discovery: `[TestClass]`, `[TestMethod]`, etc.
- Modern version: **MSTest v3** (NuGet: `MSTest`)
- Alternatives: xUnit, NUnit — similar concepts, different syntax

</v-clicks>

---

# Setting Up a Test Project

```bash
# Create a new test project
dotnet new mstest -n MyApp.Tests

# Add a reference to the project under test
dotnet add MyApp.Tests/MyApp.Tests.csproj reference MyApp/MyApp.csproj

# Run all tests
dotnet test
```

```xml
<!-- MyApp.Tests.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="MSTest" Version="3.*" />
  </ItemGroup>
</Project>
```

---

# Your First Test

```csharp
// The code we want to test
public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Subtract(int a, int b) => a - b;
}
```

```csharp
// The test class
[TestClass]
public class CalculatorTests
{
    [TestMethod]
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        var calc = new Calculator();

        int result = calc.Add(3, 4);

        Assert.AreEqual(7, result);
    }
}
```

---

# Key Attributes

The building blocks of MSTest

```csharp
[TestClass]           // marks a class as containing tests
public class MyTests
{
    [TestInitialize]  // runs before EACH test method
    public void Setup() { ... }

    [TestCleanup]     // runs after EACH test method
    public void Teardown() { ... }

    [TestMethod]      // marks a method as a test
    public void MyTest() { ... }

    [TestMethod]
    [Ignore("Not implemented yet")]
    public void SkippedTest() { ... }
}
```

---

# Class-Level Setup and Teardown

For expensive setup that runs once for the entire class

```csharp
[TestClass]
public class DatabaseTests
{
    private static SqlConnection _connection = null!;

    [ClassInitialize]   // runs ONCE before any test in the class
    public static void ClassSetup(TestContext context)
    {
        _connection = new SqlConnection("...");
        _connection.Open();
    }

    [ClassCleanup]      // runs ONCE after all tests in the class
    public static void ClassTeardown()
    {
        _connection.Dispose();
    }

    [TestMethod]
    public void QueryReturnsData() { /* uses _connection */ }
}
```

---

# The Arrange-Act-Assert Pattern

Structure every test the same way — easy to read and reason about

```csharp
[TestMethod]
public void Withdraw_SufficientFunds_ReducesBalance()
{
    // Arrange — set up the system under test and inputs
    var account = new BankAccount("Alice", initialBalance: 1000m);
    decimal withdrawAmount = 250m;

    // Act — invoke the behavior being tested
    account.Withdraw(withdrawAmount);

    // Assert — verify the expected outcome
    Assert.AreEqual(750m, account.Balance);
}
```

<v-clicks>

Keep each section visually separated. One test should verify **one behavior**.

</v-clicks>

---

# Assert.AreEqual

The most common assertion — checks expected vs actual

```csharp
[TestMethod]
public void AssertExamples()
{
    // AreEqual(expected, actual)
    Assert.AreEqual(10,      5 + 5);
    Assert.AreEqual("hello", "hel" + "lo");
    Assert.AreEqual(3.14m,   Math.Round(3.14159m, 2));

    // AreNotEqual — opposite
    Assert.AreNotEqual(0, 5 + 5);

    // Custom message shown on failure
    Assert.AreEqual(42, ComputeAnswer(), "The answer should be 42");
}
```

<v-clicks>

**Convention**: `AreEqual(expected, actual)` — expected value always comes first

</v-clicks>

---

# Assert.IsTrue and IsFalse

For boolean conditions and comparisons

```csharp
[TestMethod]
public void BooleanAssertions()
{
    var list = new List<int> { 1, 2, 3 };

    Assert.IsTrue(list.Count > 0);
    Assert.IsTrue(list.Contains(2));
    Assert.IsFalse(list.Contains(99));

    var name = "Alice";
    Assert.IsTrue(name.StartsWith("Al"));
    Assert.IsFalse(string.IsNullOrEmpty(name));

    // With a helpful failure message
    Assert.IsTrue(list.Count == 3, $"Expected 3 items but got {list.Count}");
}
```

---

# Assert.IsNull and IsNotNull

Check whether a reference holds a value

```csharp
[TestMethod]
public void NullAssertions()
{
    var repo = new UserRepository();

    // User that doesn't exist should return null
    var missing = repo.FindById(999);
    Assert.IsNull(missing);

    // User that exists should not be null
    var existing = repo.FindById(1);
    Assert.IsNotNull(existing);

    // Can also check properties
    Assert.IsNotNull(existing.Name);
    Assert.IsNotNull(existing.Email);
}
```

---

# Assert.ThrowsException

Verify that a method throws the right exception

```csharp
public class BankAccount
{
    public decimal Balance { get; private set; }
    public void Withdraw(decimal amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds");
        Balance -= amount;
    }
}

[TestMethod]
public void Withdraw_InsufficientFunds_ThrowsInvalidOperationException()
{
    var account = new BankAccount(initialBalance: 100m);

    // Assert that the exception is thrown
    Assert.ThrowsException<InvalidOperationException>(
        () => account.Withdraw(500m)
    );
}
```

---

# ThrowsException — Checking the Message

Capture the exception to inspect it further

```csharp
[TestMethod]
public void Withdraw_InsufficientFunds_MessageIsHelpful()
{
    var account = new BankAccount(initialBalance: 100m);

    var ex = Assert.ThrowsException<InvalidOperationException>(
        () => account.Withdraw(500m)
    );

    // Verify the message too
    StringAssert.Contains(ex.Message, "Insufficient funds");
}
```

```csharp
// Async version
[TestMethod]
public async Task SaveAsync_NullInput_ThrowsArgumentNullException()
{
    var service = new UserService();
    await Assert.ThrowsExceptionAsync<ArgumentNullException>(
        () => service.SaveAsync(null!)
    );
}
```

---

# Asserting Floating-Point Values

Floating-point math is imprecise — use a tolerance (delta)

```csharp
[TestMethod]
public void FloatingPointAssertions()
{
    double result = 0.1 + 0.2;

    // This FAILS — floating point is not exact
    // Assert.AreEqual(0.3, result);

    // Use a delta (tolerance)
    Assert.AreEqual(0.3, result, delta: 0.0000001);

    // Another example
    double area = Math.PI * 5 * 5; // ~78.5398
    Assert.AreEqual(78.54, area, delta: 0.01);
}
```

---

# CollectionAssert

Assertions specifically for collections

```csharp
[TestMethod]
public void CollectionAssertions()
{
    var result  = new List<int> { 3, 1, 4, 1, 5 };
    var sorted  = new List<int> { 1, 1, 3, 4, 5 };
    var empty   = new List<int>();

    CollectionAssert.Contains(result, 4);
    CollectionAssert.DoesNotContain(result, 99);
    CollectionAssert.AreEqual(sorted, result.OrderBy(x => x).ToList());
    CollectionAssert.AreEquivalent(result, new[] { 5, 4, 3, 1, 1 }); // order-insensitive

    // Count checks
    Assert.AreEqual(5, result.Count);
    CollectionAssert.AllItemsAreNotNull(result);
    CollectionAssert.AllItemsAreUnique(new[] { 1, 2, 3 });
}
```

---

# StringAssert

Assertions specifically for strings

```csharp
[TestMethod]
public void StringAssertions()
{
    string greeting = "Hello, World!";
    string email    = "alice@example.com";

    StringAssert.Contains(greeting, "World");
    StringAssert.StartsWith(greeting, "Hello");
    StringAssert.EndsWith(greeting, "!");

    // Regex matching
    StringAssert.Matches(email, new System.Text.RegularExpressions.Regex(
        @"^[^@]+@[^@]+\.[^@]+$"));

    StringAssert.DoesNotMatch("not-an-email", new System.Text.RegularExpressions.Regex(
        @"^[^@]+@[^@]+\.[^@]+$"));
}
```

---

# Parameterized Tests with DataRow

Run the same test logic with multiple inputs

```csharp
[TestClass]
public class CalculatorTests
{
    [DataTestMethod]
    [DataRow(2,  3,  5)]
    [DataRow(0,  0,  0)]
    [DataRow(-1, 1,  0)]
    [DataRow(10, -3, 7)]
    public void Add_VariousInputs_ReturnsCorrectSum(int a, int b, int expected)
    {
        var calc = new Calculator();

        int result = calc.Add(a, b);

        Assert.AreEqual(expected, result);
    }
}
```

<v-clicks>

Each `[DataRow]` becomes a separate test entry in the test runner — easy to see which cases fail

</v-clicks>

---

# DataRow with Strings and Objects

DataRow works with any compile-time constant

```csharp
[DataTestMethod]
[DataRow("hello",   "HELLO")]
[DataRow("World",   "WORLD")]
[DataRow("",        "")]
[DataRow("cShArP",  "CSHARP")]
public void ToUpper_ReturnsUppercaseString(string input, string expected)
{
    string result = input.ToUpper();
    Assert.AreEqual(expected, result);
}

[DataTestMethod]
[DataRow("alice@example.com", true)]
[DataRow("not-an-email",      false)]
[DataRow("",                  false)]
[DataRow("a@b.c",             true)]
public void IsValidEmail_ReturnsExpectedResult(string email, bool expected)
{
    bool result = EmailValidator.IsValid(email);
    Assert.AreEqual(expected, result);
}
```

---

# DynamicData — Runtime Test Data

When data can't be expressed as compile-time constants

```csharp
[TestClass]
public class MathTests
{
    public static IEnumerable<object[]> PrimeNumbers =>
        new[]
        {
            new object[] { 2 },
            new object[] { 3 },
            new object[] { 5 },
            new object[] { 13 },
            new object[] { 97 },
        };

    [DataTestMethod]
    [DynamicData(nameof(PrimeNumbers))]
    public void IsPrime_KnownPrimes_ReturnsTrue(int number)
    {
        bool result = MathHelper.IsPrime(number);
        Assert.IsTrue(result, $"{number} should be prime");
    }
}
```

---

# TestContext — Test Metadata

Access information about the currently running test

```csharp
[TestClass]
public class LoggingTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestInitialize]
    public void Setup()
    {
        TestContext.WriteLine($"Starting: {TestContext.TestName}");
    }

    [TestMethod]
    public void SomeTest()
    {
        TestContext.WriteLine("Doing work...");
        // Write files or output to test results
        // TestContext.AddResultFile("output.txt");
        Assert.IsTrue(true);
    }
}
```

---

# TestCategory — Organizing Tests

Group tests so you can run subsets

```csharp
[TestClass]
public class OrderServiceTests
{
    [TestMethod]
    [TestCategory("Unit")]
    public void CalculateTotal_AppliesDiscountCorrectly() { ... }

    [TestMethod]
    [TestCategory("Integration")]
    [TestCategory("Database")]
    public void SaveOrder_PersistsToDatabase() { ... }

    [TestMethod]
    [TestCategory("Smoke")]
    public void CreateOrder_BasicFlow_Succeeds() { ... }
}
```

```bash
# Run only unit tests in CI
dotnet test --filter "TestCategory=Unit"

# Run smoke tests before a deployment
dotnet test --filter "TestCategory=Smoke"
```

---

# Testing Async Methods

Use `async Task` — never `async void` for test methods

```csharp
public class WeatherService
{
    public async Task<string> GetForecastAsync(string city)
    {
        await Task.Delay(10); // simulate async work
        return $"Sunny in {city}";
    }
}

[TestClass]
public class WeatherServiceTests
{
    [TestMethod]
    public async Task GetForecastAsync_ValidCity_ReturnsForecast()
    {
        var service = new WeatherService();

        string result = await service.GetForecastAsync("Seattle");

        StringAssert.Contains(result, "Seattle");
    }
}
```

---

# Testing with Interfaces and Fakes

Design code with interfaces so dependencies can be swapped in tests

```csharp
// Production code
public interface IEmailSender
{
    void Send(string to, string subject, string body);
}

public class UserService
{
    private readonly IEmailSender _emailSender;
    public UserService(IEmailSender emailSender) => _emailSender = emailSender;

    public void Register(string email)
    {
        // ... save user ...
        _emailSender.Send(email, "Welcome!", "Thanks for signing up.");
    }
}
```

```csharp
// Test fake — records calls without real email sending
public class FakeEmailSender : IEmailSender
{
    public List<string> SentTo { get; } = new();
    public void Send(string to, string subject, string body) => SentTo.Add(to);
}
```

---

# Using the Fake in Tests

```csharp
[TestClass]
public class UserServiceTests
{
    [TestMethod]
    public void Register_ValidEmail_SendsWelcomeEmail()
    {
        // Arrange
        var fakeEmail = new FakeEmailSender();
        var service   = new UserService(fakeEmail);

        // Act
        service.Register("alice@example.com");

        // Assert
        Assert.AreEqual(1, fakeEmail.SentTo.Count);
        Assert.AreEqual("alice@example.com", fakeEmail.SentTo[0]);
    }

    [TestMethod]
    public void Register_TwoUsers_SendsTwoEmails()
    {
        var fakeEmail = new FakeEmailSender();
        var service   = new UserService(fakeEmail);

        service.Register("alice@example.com");
        service.Register("bob@example.com");

        Assert.AreEqual(2, fakeEmail.SentTo.Count);
    }
}
```

---

# Mocking with Moq

Moq generates fake implementations at runtime — no manual fake class needed

```bash
dotnet add package Moq
```

```csharp
using Moq;

[TestMethod]
public void Register_ValidEmail_SendsWelcomeEmail()
{
    // Arrange — create a mock of the interface
    var mockEmail = new Mock<IEmailSender>();
    var service   = new UserService(mockEmail.Object);

    // Act
    service.Register("alice@example.com");

    // Assert — verify the method was called with the right arguments
    mockEmail.Verify(
        e => e.Send("alice@example.com", "Welcome!", It.IsAny<string>()),
        Times.Once
    );
}
```

---

# Moq — Setup Return Values

Configure mocks to return specific values

```csharp
public interface IUserRepository
{
    User? FindByEmail(string email);
    void Save(User user);
}

[TestMethod]
public void Login_ExistingUser_ReturnsToken()
{
    var mockRepo = new Mock<IUserRepository>();

    // Setup — when FindByEmail is called with this value, return this user
    mockRepo.Setup(r => r.FindByEmail("alice@example.com"))
            .Returns(new User { Id = 1, Name = "Alice", Email = "alice@example.com" });

    var service = new AuthService(mockRepo.Object);
    string token = service.Login("alice@example.com", "password123");

    Assert.IsNotNull(token);
}
```

---

# Moq — It.IsAny and It.Is

Flexible argument matching

```csharp
[TestMethod]
public void SaveUser_AnyValidUser_CallsRepository()
{
    var mockRepo = new Mock<IUserRepository>();

    // Match any string argument
    mockRepo.Setup(r => r.FindByEmail(It.IsAny<string>()))
            .Returns((User?)null);

    var service = new UserService(mockRepo.Object);
    service.Register("new@example.com");

    // Verify Save was called with a user whose email matches
    mockRepo.Verify(
        r => r.Save(It.Is<User>(u => u.Email == "new@example.com")),
        Times.Once
    );
}
```

---

# Moq — Setup Exceptions

Test error paths by configuring mocks to throw

```csharp
[TestMethod]
public void GetUser_RepositoryThrows_ReturnsNull()
{
    var mockRepo = new Mock<IUserRepository>();

    // Configure the mock to throw an exception
    mockRepo.Setup(r => r.FindByEmail(It.IsAny<string>()))
            .Throws(new TimeoutException("Database timeout"));

    var service = new UserService(mockRepo.Object);

    // Service should handle the exception gracefully
    var result = service.TryGetUser("alice@example.com");

    Assert.IsNull(result);
}
```

---

# Moq — Async Methods

Mock async methods with `ReturnsAsync`

```csharp
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task SaveAsync(Product product);
}

[TestMethod]
public async Task GetProduct_ExistingId_ReturnsProduct()
{
    var mockRepo = new Mock<IProductRepository>();
    mockRepo.Setup(r => r.GetByIdAsync(42))
            .ReturnsAsync(new Product { Id = 42, Name = "Widget" });

    var service = new ProductService(mockRepo.Object);
    var product = await service.GetProductAsync(42);

    Assert.IsNotNull(product);
    Assert.AreEqual("Widget", product.Name);
}
```

---

# Testing a Complete Service

A realistic end-to-end test of a business service

```csharp
public class OrderService
{
    private readonly IOrderRepository _repo;
    private readonly IInventory _inventory;

    public OrderService(IOrderRepository repo, IInventory inventory)
    { _repo = repo; _inventory = inventory; }

    public OrderResult PlaceOrder(int productId, int quantity)
    {
        if (!_inventory.IsAvailable(productId, quantity))
            return OrderResult.OutOfStock;
        var order = new Order { ProductId = productId, Quantity = quantity };
        _repo.Save(order);
        _inventory.Reserve(productId, quantity);
        return OrderResult.Success;
    }
}
```

---

# Testing the Order Service

```csharp
[TestClass]
public class OrderServiceTests
{
    private Mock<IOrderRepository> _mockRepo    = null!;
    private Mock<IInventory>       _mockInventory = null!;
    private OrderService           _service     = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo      = new Mock<IOrderRepository>();
        _mockInventory = new Mock<IInventory>();
        _service       = new OrderService(_mockRepo.Object, _mockInventory.Object);
    }

    [TestMethod]
    public void PlaceOrder_ItemInStock_ReturnsSuccess()
    {
        _mockInventory.Setup(i => i.IsAvailable(1, 5)).Returns(true);

        var result = _service.PlaceOrder(productId: 1, quantity: 5);

        Assert.AreEqual(OrderResult.Success, result);
        _mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Once);
    }

    [TestMethod]
    public void PlaceOrder_OutOfStock_ReturnsOutOfStock()
    {
        _mockInventory.Setup(i => i.IsAvailable(1, 100)).Returns(false);

        var result = _service.PlaceOrder(productId: 1, quantity: 100);

        Assert.AreEqual(OrderResult.OutOfStock, result);
        _mockRepo.Verify(r => r.Save(It.IsAny<Order>()), Times.Never);
    }
}
```

---

# Test Naming Conventions

Good test names are self-documenting

```csharp
// Pattern: MethodName_Scenario_ExpectedResult

[TestMethod]
public void Add_TwoPositiveNumbers_ReturnsSum() { }

[TestMethod]
public void Withdraw_InsufficientFunds_ThrowsInvalidOperationException() { }

[TestMethod]
public void FindUser_UserDoesNotExist_ReturnsNull() { }

[TestMethod]
public void Login_CorrectCredentials_ReturnsAuthToken() { }

[TestMethod]
public void Login_WrongPassword_ThrowsUnauthorizedException() { }

[TestMethod]
public void SendEmail_NullAddress_ThrowsArgumentNullException() { }
```

<v-clicks>

A failing test name should tell you exactly what broke without reading the code

</v-clicks>

---

# One Assert Per Test (Mostly)

Focused tests give clearer failure messages

```csharp
// BAD — multiple behaviors tested at once
[TestMethod]
public void Register_Test()
{
    var service = new UserService(new FakeEmailSender());
    service.Register("alice@example.com");
    // if this fails, which behavior is broken?
    Assert.AreEqual(1, service.UserCount);
    Assert.IsTrue(service.UserExists("alice@example.com"));
}

// BETTER — one behavior per test
[TestMethod]
public void Register_NewUser_IncreasesUserCount() { ... }

[TestMethod]
public void Register_NewUser_UserExistsAfterward() { ... }

[TestMethod]
public void Register_NewUser_SendsWelcomeEmail() { ... }
```

---

# Test-Driven Development (TDD)

Write the test *before* the code — then make it pass

<v-clicks>

### Red — Green — Refactor

1. **Red**: Write a failing test for the behavior you want
2. **Green**: Write the minimum code to make the test pass
3. **Refactor**: Clean up the code without breaking the test

</v-clicks>

<v-clicks>

### Why TDD?
- Forces you to think about the API before implementation
- Guarantees 100% test coverage of new code
- Leads to smaller, more focused functions
- Builds a safety net as you go

</v-clicks>

---

# TDD Example

```csharp
// Step 1: RED — write a failing test
[TestMethod]
public void FizzBuzz_MultiplesOf3_ReturnsFizz()
{
    Assert.AreEqual("Fizz", FizzBuzz.Convert(3));   // doesn't exist yet!
    Assert.AreEqual("Fizz", FizzBuzz.Convert(9));
}

// Step 2: GREEN — write just enough to pass
public static class FizzBuzz
{
    public static string Convert(int n) => n % 3 == 0 ? "Fizz" : n.ToString();
}

// Step 3: REFACTOR — add more cases, keeping tests green
public static string Convert(int n) =>
    (n % 15 == 0) ? "FizzBuzz" :
    (n %  3 == 0) ? "Fizz"     :
    (n %  5 == 0) ? "Buzz"     :
    n.ToString();
```

---

# Fakes vs Stubs vs Mocks

Three types of test doubles — often used interchangeably but distinct

<v-clicks>

### Stub
Returns pre-canned data — doesn't verify calls
```csharp
mockRepo.Setup(r => r.FindById(1)).Returns(new User()); // stub
```

### Mock
Verifies that certain calls were (or weren't) made
```csharp
mockEmail.Verify(e => e.Send(...), Times.Once); // mock assertion
```

### Fake
A real but simplified implementation (like `FakeEmailSender`)
```csharp
public class FakeEmailSender : IEmailSender { ... } // fake
```

</v-clicks>

---

# Common Pitfalls

```csharp
// 1. Testing implementation details instead of behavior
// BAD: testing that a private method was called
// GOOD: test the observable output

// 2. Brittle tests that break on refactoring
mockRepo.Verify(r => r.FindById(It.IsAny<int>()), Times.Exactly(3)); // magic number
// BETTER: verify the result, not the call count (unless count matters)

// 3. Shared mutable state between tests
private static List<User> _users = new();   // shared — tests can pollute each other
// BETTER: create fresh instances in [TestInitialize]

// 4. Async void test methods — exceptions are swallowed!
[TestMethod]
public async void BadAsyncTest() { ... }  // WRONG
[TestMethod]
public async Task GoodAsyncTest() { ... } // CORRECT
```

---

# Common Pitfalls (Continued)

```csharp
// 5. Testing too much in one test — hard to diagnose failures
[TestMethod]
public void EverythingTest()
{
    // 30 assertions...
}

// 6. Tests that depend on order
// MSTest does NOT guarantee test execution order
// Each test must be fully independent

// 7. Not testing edge cases
[DataRow(0)]          // zero
[DataRow(-1)]         // negative
[DataRow(int.MaxValue)] // boundary
public void Method_EdgeCases(int input) { ... }

// 8. Hardcoded dates/times — tests that fail at midnight or on New Year's
// Inject a clock interface instead of using DateTime.Now directly
```

---

# Measuring Code Coverage

See how much of your code is exercised by tests

```bash
# Install the coverage tool
dotnet tool install -g dotnet-coverage

# Collect coverage
dotnet-coverage collect "dotnet test" -f xml -o coverage.xml

# Generate an HTML report
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:coverage.xml -targetdir:coveragereport -reporttypes:Html

# Open coveragereport/index.html in a browser
```

<v-clicks>

**100% coverage doesn't mean bug-free** — focus on testing meaningful behaviors, not just hitting lines.

Aim for **high coverage of business logic**, not getters/setters.

</v-clicks>

---

# Running Tests from the CLI

```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"

# Filter by test name
dotnet test --filter "FullyQualifiedName~CalculatorTests"

# Filter by category
dotnet test --filter "TestCategory=Unit"

# Filter by method name
dotnet test --filter "Name=Add_TwoPositiveNumbers_ReturnsSum"

# Run tests in parallel
dotnet test -- MSTest.Parallelize.Workers=4

# Output results as TRX (for CI)
dotnet test --logger "trx;LogFileName=results.trx"
```

---

# Best Practices

<v-clicks>

- **Follow AAA** — Arrange, Act, Assert — one section per concern
- **One behavior per test** — make failures easy to diagnose
- **Use descriptive names** — `MethodName_Scenario_ExpectedResult`
- **Avoid test logic** — no `if`/`for` in tests; use `[DataRow]` instead
- **Keep tests fast** — unit tests should run in milliseconds
- **Don't test framework code** — don't test getters, setters, or constructors that just assign
- **Use `[TestInitialize]`** to create fresh objects — never share mutable state
- **Test edge cases** — nulls, empty collections, boundaries, negatives
- **Use interfaces** for dependencies so they can be mocked
- **Treat test code like production code** — refactor and keep it clean

</v-clicks>

---

# Summary

<v-clicks>

- `[TestClass]` + `[TestMethod]` — mark classes and methods for discovery
- `[TestInitialize]` / `[TestCleanup]` — per-test setup and teardown
- `[ClassInitialize]` / `[ClassCleanup]` — once-per-class setup
- `Assert.*` — verify expected vs actual values, nulls, exceptions, booleans
- `CollectionAssert.*` / `StringAssert.*` — specialized collection and string checks
- `[DataTestMethod]` + `[DataRow]` — parameterized tests
- `[DynamicData]` — runtime test data from a property or method
- **Fakes and Mocks** — swap real dependencies with test doubles
- **Moq** — generate mocks at runtime with `Setup`, `Returns`, `Verify`
- **TDD**: Red → Green → Refactor

</v-clicks>

---

# Questions?

## Topics We Covered

- Why test and types of tests
- MSTest project setup and key attributes
- Arrange-Act-Assert pattern
- `Assert`, `CollectionAssert`, `StringAssert`
- Testing exceptions and async code
- Parameterized tests with `[DataRow]` and `[DynamicData]`
- `[TestCategory]` and test filtering
- Fakes, stubs, and mocks
- Mocking with Moq (`Setup`, `Returns`, `Verify`, `ReturnsAsync`)
- Test-driven development (TDD)
- Code coverage and CLI usage
- Naming conventions and best practices
