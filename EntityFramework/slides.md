---
theme: apple-basic
colorSchema: light
info: |
  ## Entity Framework Core
  A comprehensive guide to EF Core for C# developers
drawings:
  persist: false
transition: slide-left
title: Entity Framework Core
mdc: true
---

<style src="./style.css"></style>

# Entity Framework Core

A Modern ORM for .NET Applications

<div class="pt-12">
  <span @click="$slidev.nav.next" class="px-2 py-1 rounded cursor-pointer" hover="bg-white bg-opacity-10">
    Press Space for next page <carbon:arrow-right class="inline"/>
  </span>
</div>

---

# Introduction to Entity Framework

**Entity Framework (EF) Core** is a lightweight, extensible, open-source, and cross-platform ORM (Object-Relational Mapper) for .NET.

## What is an ORM?

An Object-Relational Mapper bridges the gap between your object-oriented code and relational databases, allowing you to work with databases using C# objects instead of writing SQL.

## Key Benefits

- ✅ Reduces boilerplate ADO.NET code
- ✅ Type-safe database operations
- ✅ Automatic SQL generation
- ✅ Cross-database compatibility
- ✅ Strong community and ecosystem support

---

# EF Core Key Features

What makes Entity Framework Core powerful

- 🎯 **LINQ Support** - Write type-safe queries using LINQ
- 🔄 **Change Tracking** - Automatic detection of entity changes
- 💾 **Multiple Providers** - SQL Server, PostgreSQL, SQLite, In-Memory, and more
- 🧪 **Testing Support** - In-memory database for unit testing
- 🔧 **Migrations** - Code-first database schema management
- ⚡ **Performance** - Optimized queries and connection pooling

---

# EF Core and LINQ

Entity Framework leverages **LINQ** (Language Integrated Query) for querying databases

## Traditional SQL vs LINQ

<div class="grid grid-cols-2 gap-4">
<div>

### Traditional SQL
```csharp
string sql = @"
  SELECT * FROM Products 
  WHERE Price > 100 
  ORDER BY Name";
var products = ExecuteQuery(sql);
```

**Issues:**
- No compile-time checking
- String concatenation risks
- Hard to refactor
- Prone to SQL injection

</div>
<div>

### LINQ with EF
```csharp
var products = await context.Products
  .Where(p => p.Price > 100)
  .OrderBy(p => p.Name)
  .ToListAsync();
```

**Benefits:**
- ✅ Type-safe
- ✅ IntelliSense support
- ✅ Compile-time validation
- ✅ Easy to refactor
- ✅ Protected from SQL injection

</div>
</div>

---

# Basic Query Examples

Common query patterns with Entity Framework Core

```csharp
// Basic select all
var allProducts = await context.Products.ToListAsync();

// Filtering with Where
var expensiveProducts = await context.Products
    .Where(p => p.Price > 100)
    .ToListAsync();

// Ordering results
var sortedProducts = await context.Products
    .OrderBy(p => p.Name)
    .ThenByDescending(p => p.Price)
    .ToListAsync();

// Projecting to anonymous types
var productSummaries = await context.Products
    .Select(p => new {
        p.Name,
        p.Price,
        DiscountedPrice = p.Price * 0.9m
    })
    .ToListAsync();

// Pagination
var page2 = await context.Products
    .OrderBy(p => p.Id)
    .Skip(10)
    .Take(10)
    .ToListAsync();
```

> **Best Practice:** Always use async methods (`ToListAsync`, `FirstAsync`, etc.) for scalability

---

# More Query Examples

Advanced querying techniques

<div class="grid grid-cols-2 gap-4">
<div>

### First/Single Operations
```csharp
// First matching or exception
var product = await context.Products
    .FirstAsync(p => p.Id == 1);

// First or null
var product = await context.Products
    .FirstOrDefaultAsync(p => p.Id == 1);

// Single (throws if multiple)
var product = await context.Products
    .SingleAsync(p => p.Sku == "ABC123");
```

### Existence Checks
```csharp
bool hasExpensive = await context.Products
    .AnyAsync(p => p.Price > 1000);

bool allInStock = await context.Products
    .AllAsync(p => p.InStock);
```

</div>
<div>

### Counting
```csharp
int total = await context.Products
    .CountAsync();

int expensive = await context.Products
    .CountAsync(p => p.Price > 100);
```

### Distinct Values
```csharp
var categories = await context.Products
    .Select(p => p.Category)
    .Distinct()
    .ToListAsync();
```

### Min/Max
```csharp
var cheapest = await context.Products
    .MinAsync(p => p.Price);
    
var expensive = await context.Products
    .MaxAsync(p => p.Price);
```

</div>
</div>

---

# Insert Examples

Adding new entities to the database

```csharp
// Create and add a single product
var product = new Product
{
    Name = "Gaming Laptop",
    Price = 1299.99m,
    InStock = true,
    Category = "Electronics"
};

context.Products.Add(product);
await context.SaveChangesAsync(); // Commits to database
// product.Id is now populated with database-generated ID

// Add multiple entities at once
var products = new List<Product>
{
    new Product { Name = "Mouse", Price = 29.99m },
    new Product { Name = "Keyboard", Price = 79.99m },
    new Product { Name = "Monitor", Price = 299.99m }
};

context.Products.AddRange(products);
await context.SaveChangesAsync();

// Insert with related entities (EF handles foreign keys)
var category = new Category { Name = "Electronics" };
var laptop = new Product 
{ 
    Name = "Laptop",
    Price = 999.99m,
    Category = category // EF will insert category first, then product
};

context.Products.Add(laptop);
await context.SaveChangesAsync(); // Both inserted in correct order
```

---

# Update Examples

Modifying existing entities

### Tracked Entity Update
```csharp
// Retrieve entity (automatically tracked by context)
var product = await context.Products
    .FirstAsync(p => p.Id == 1);

// Modify properties
product.Price = 899.99m;
product.InStock = true;

// SaveChanges automatically detects changes
await context.SaveChangesAsync();
```

### Untracked Update
```csharp
// Update without loading from database first
var product = new Product
{
    Id = 1,
    Name = "Updated Name",
    Price = 799.99m,
    InStock = true
};

context.Products.Update(product);
await context.SaveChangesAsync();
// ⚠️ This updates ALL properties - be careful!
```

---

# More Update Examples

Advanced update techniques

### Partial Update
```csharp
// Update only specific properties
var product = await context.Products
    .FirstAsync(p => p.Id == 1);

// Update specific property only
context.Entry(product)
    .Property(p => p.Price)
    .CurrentValue = 699.99m;

await context.SaveChangesAsync();
```

### Bulk Update (EF Core 7+)
```csharp
// Update multiple records without loading them
// No change tracking overhead - executes directly on database
await context.Products
    .Where(p => p.Category == "Electronics")
    .ExecuteUpdateAsync(
        s => s.SetProperty(
            p => p.Price, 
            p => p.Price * 0.9m  // 10% discount
        )
    );

// Multiple property updates
await context.Products
    .Where(p => p.Discontinued)
    .ExecuteUpdateAsync(s => s
        .SetProperty(p => p.InStock, false)
        .SetProperty(p => p.Price, 0));
```

---

# Delete Examples

Removing entities from the database

```csharp
// Delete tracked entity
var product = await context.Products
    .FirstAsync(p => p.Id == 1);

context.Products.Remove(product);
await context.SaveChangesAsync();

// Delete without loading (more efficient)
var product = new Product { Id = 1 };
context.Products.Remove(product);
await context.SaveChangesAsync();

// Delete multiple entities
var oldProducts = await context.Products
    .Where(p => p.Price < 10)
    .ToListAsync();

context.Products.RemoveRange(oldProducts);
await context.SaveChangesAsync();

// Bulk Delete (EF Core 7+) - Most efficient
await context.Products
    .Where(p => p.Discontinued)
    .ExecuteDeleteAsync(); // Executes immediately, no tracking
```

> **Performance Tip:** Bulk operations (`ExecuteDeleteAsync`, `ExecuteUpdateAsync`) are more efficient as they execute directly on the database without loading entities into memory

---

# Repository Pattern

Abstraction layer for data access logic

## Why Use Repository Pattern?

- ✅ Separates data access from business logic
- ✅ Makes code more testable (easier to mock)
- ✅ Centralizes query logic
- ✅ Enables dependency injection
- ✅ Provides consistent API across entities

## When to Use?

- Complex applications with multiple data sources
- When you need to swap implementations (testing, different databases)
- Team projects requiring consistent data access patterns
- Applications requiring extensive unit testing

---

# Repository Interface

Defining the contract for data access

```csharp
public interface IRepository<T> where T : class
{
    // Query operations
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    
    // Command operations
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    
    // Persistence
    Task<int> SaveChangesAsync();
}
```

> This interface can be used with any entity type in your application

---

# Repository Implementation

Generic repository implementation

```csharp
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    public virtual async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public virtual void Update(T entity)
        => _dbSet.Update(entity);

    public virtual void Remove(T entity)
        => _dbSet.Remove(entity);

    public virtual async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
}
```

---

# Unit of Work Pattern

Coordinating multiple repositories in a single transaction

## What is Unit of Work?

A pattern that maintains a list of objects affected by a business transaction and coordinates the writing of changes and resolution of concurrency problems.

## Key Benefits

- ✅ Groups multiple operations into a single transaction
- ✅ Ensures data consistency across repositories
- ✅ Single point of save/commit for all changes
- ✅ Simplifies transaction management
- ✅ Reduces database round trips

---

# Unit of Work Interface

Defining the contract for coordinated data access

```csharp
public interface IUnitOfWork : IDisposable
{
    // Repository properties - access to all repositories
    IRepository<Product> Products { get; }
    IRepository<Category> Categories { get; }
    IRepository<Order> Orders { get; }
    IRepository<Customer> Customers { get; }
    
    // Persistence operations
    Task<int> SaveChangesAsync();
}
```

**Key Concepts:**
- Single interface provides access to multiple repositories
- `SaveChangesAsync()` commits all pending changes across all repositories
- Implements `IDisposable` for proper resource cleanup

---

# Unit of Work Transactions

Adding transaction support to Unit of Work

```csharp
public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }
    IRepository<Category> Categories { get; }
    IRepository<Order> Orders { get; }
    
    Task<int> SaveChangesAsync();
    
    // Explicit transaction management
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
```

**Transaction Methods:**
- `BeginTransactionAsync()` - Start a new database transaction
- `CommitTransactionAsync()` - Commit all changes permanently
- `RollbackTransactionAsync()` - Undo all changes in the transaction

> Transactions ensure that multiple operations either all succeed or all fail together

---

# Unit of Work Implementation

Creating the implementation class

```csharp
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Products = new Repository<Product>(_context);
        Categories = new Repository<Category>(_context);
        Orders = new Repository<Order>(_context);
    }

    public IRepository<Product> Products { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Order> Orders { get; }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task BeginTransactionAsync()
        => _transaction = await _context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
        => await _transaction?.CommitAsync()!;

    public async Task RollbackTransactionAsync()
        => await _transaction?.RollbackAsync()!;

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
```

---

# Unit of Work Usage Example

Practical example with transaction management

```csharp
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task TransferInventory(int fromId, int toId, int quantity)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Get products from repository
            var from = await _unitOfWork.Products.GetByIdAsync(fromId);
            var to = await _unitOfWork.Products.GetByIdAsync(toId);

            // Modify inventory
            from.Quantity -= quantity;
            to.Quantity += quantity;

            // Single save for all changes
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            // Rollback on any error - maintains data consistency
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
```

> All changes are committed or rolled back together, ensuring data consistency

---

# Multiple Database Providers

EF Core supports various database systems with provider-specific packages

<div class="grid grid-cols-2 gap-4">
<div>

### SQL Server
```csharp
// Package: Microsoft.EntityFrameworkCore.SqlServer
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
```

### PostgreSQL
```csharp
// Package: Npgsql.EntityFrameworkCore.PostgreSQL
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
```

### SQLite
```csharp
// Package: Microsoft.EntityFrameworkCore.Sqlite
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));
```

</div>
<div>

### MySQL
```csharp
// Package: Pomelo.EntityFrameworkCore.MySql
services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString)));
```

### Cosmos DB
```csharp
// Package: Microsoft.EntityFrameworkCore.Cosmos
services.AddDbContext<AppDbContext>(options =>
    options.UseCosmos(endpoint, key, dbName));
```

### In-Memory
```csharp
// Package: Microsoft.EntityFrameworkCore.InMemory
services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));
```

</div>
</div>

> You can even use multiple providers in the same application with separate contexts!

---

# EF Core with In-Memory Database

Perfect for unit testing and rapid prototyping

## Setup and Configuration

```csharp
// Install: Microsoft.EntityFrameworkCore.InMemory

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }
}

// Create in-memory context
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(databaseName: "TestDb")
    .Options;

using var context = new AppDbContext(options);
```

## Benefits and Limitations

**Benefits:**
- ✅ No database setup required
- ✅ Fast test execution
- ✅ Isolated test data
- ✅ No cleanup needed
- ✅ Easy CI/CD integration

**Limitations:**
- ❌ No referential integrity enforcement
- ❌ No SQL-specific features (stored procedures, etc.)
- ❌ Limited query capabilities
- ❌ Not suitable for production use

---

# Unit Testing with EF Core

Writing testable data access code

```csharp
public class ProductServiceTests
{
    private AppDbContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetExpensiveProducts_ReturnsCorrectProducts()
    {
        // Arrange
        using var context = GetInMemoryContext();
        context.Products.AddRange(
            new Product { Name = "Cheap", Price = 10 },
            new Product { Name = "Expensive", Price = 200 },
            new Product { Name = "Very Expensive", Price = 500 }
        );
        await context.SaveChangesAsync();
        var service = new ProductService(context);

        // Act
        var results = await service.GetExpensiveProductsAsync(100);

        // Assert
        Assert.Equal(2, results.Count());
        Assert.All(results, p => Assert.True(p.Price > 100));
    }
}
```

> **Tip:** Use `Guid.NewGuid().ToString()` as database name for isolated tests

---

# EF Core Joins

Querying related data across multiple tables

<div class="grid grid-cols-2 gap-4">
<div>

### Inner Join (LINQ Query)
```csharp
var query = from product in context.Products
            join category in context.Categories
                on product.CategoryId 
                equals category.Id
            select new
            {
                ProductName = product.Name,
                CategoryName = category.Name
            };
```

### Inner Join (Method Syntax)
```csharp
var query = context.Products
    .Join(context.Categories,
        p => p.CategoryId,
        c => c.Id,
        (p, c) => new
        {
            ProductName = p.Name,
            CategoryName = c.Name
        });
```

</div>
<div>

### Include (Eager Loading)
```csharp
// Load related entities
var products = await context.Products
    .Include(p => p.Category)
    .Include(p => p.Supplier)
    .ToListAsync();

// Multi-level include
var orders = await context.Orders
    .Include(o => o.Customer)
    .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Product)
    .ToListAsync();
```

### Left Join
```csharp
var query = from p in context.Products
            join c in context.Categories
                on p.CategoryId equals c.Id
                into categoryGroup
            from c in categoryGroup
                .DefaultIfEmpty()
            select new
            {
                ProductName = p.Name,
                CategoryName = c.Name ?? "N/A"
            };
```

</div>
</div>

---

# EF Core Aggregation

Computing summary values and statistics

<div class="grid grid-cols-2 gap-4">
<div>

### Basic Aggregations
```csharp
// Count
var total = await context.Products
    .CountAsync();

// Sum
var totalValue = await context.Products
    .SumAsync(p => p.Price * p.Quantity);

// Average
var avgPrice = await context.Products
    .AverageAsync(p => p.Price);

// Min/Max
var cheapest = await context.Products
    .MinAsync(p => p.Price);
var expensive = await context.Products
    .MaxAsync(p => p.Price);
```

### Grouping
```csharp
var categoryStats = await context.Products
    .GroupBy(p => p.Category)
    .Select(g => new
    {
        Category = g.Key,
        Count = g.Count(),
        AvgPrice = g.Average(p => p.Price),
        TotalValue = g.Sum(p => p.Price)
    })
    .ToListAsync();
```

</div>
<div>

### Having Clause
```csharp
var popularCategories = 
    await context.Products
    .GroupBy(p => p.Category)
    .Where(g => g.Count() > 10)
    .Select(g => new
    {
        Category = g.Key,
        ProductCount = g.Count()
    })
    .ToListAsync();
```

### Complex Aggregations
```csharp
var summary = await context.Orders
    .GroupBy(o => new 
    { 
        o.CustomerId, 
        Year = o.OrderDate.Year 
    })
    .Select(g => new
    {
        g.Key.CustomerId,
        g.Key.Year,
        OrderCount = g.Count(),
        TotalAmount = g.Sum(o => o.Total),
        AvgOrder = g.Average(o => o.Total),
        MaxOrder = g.Max(o => o.Total)
    })
    .ToListAsync();
```

</div>
</div>

---

# Custom SQL Queries

When LINQ isn't sufficient for complex scenarios

<div class="grid grid-cols-2 gap-4">
<div>

### FromSqlRaw
```csharp
var products = await context.Products
    .FromSqlRaw(@"
        SELECT * FROM Products 
        WHERE Price > {0}", 100)
    .ToListAsync();

// Composable with LINQ
var filtered = await context.Products
    .FromSqlRaw("SELECT * FROM Products")
    .Where(p => p.InStock)
    .OrderBy(p => p.Name)
    .ToListAsync();
```

### FromSqlInterpolated (Safer)
```csharp
var minPrice = 100m;
var products = await context.Products
    .FromSqlInterpolated($@"
        SELECT * FROM Products 
        WHERE Price > {minPrice}")
    .ToListAsync();
// Automatically parameterized
```

</div>
<div>

### ExecuteSqlRaw
```csharp
// For UPDATE/DELETE operations
var affected = await context.Database
    .ExecuteSqlRawAsync(@"
        UPDATE Products 
        SET Price = Price * 1.1 
        WHERE CategoryId = {0}", 
        categoryId);
```

### Stored Procedures
```csharp
var products = await context.Products
    .FromSqlRaw(
        "EXEC GetProductsByCategory {0}", 
        categoryId)
    .ToListAsync();

// With output parameters
var outputParam = new SqlParameter
{
    ParameterName = "@TotalCount",
    SqlDbType = SqlDbType.Int,
    Direction = ParameterDirection.Output
};

await context.Database.ExecuteSqlRawAsync(
    "EXEC GetStats @CategoryId, @TotalCount OUT",
    new SqlParameter("@CategoryId", categoryId),
    outputParam);
```

</div>
</div>

---

# EF Core Configuration

Configuring DbContext and entity behavior

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer("ConnectionString")
                .EnableSensitiveDataLogging() // ⚠️ Development only!
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.Sku).IsUnique();
            
            // Relationships
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Apply all configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```

---

# Entity Configuration Classes

Separating configuration logic for better organization

<div class="grid grid-cols-2 gap-4">
<div>

### Configuration Class
```csharp
public class ProductConfiguration 
    : IEntityTypeConfiguration<Product>
{
    public void Configure(
        EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasIndex(p => p.Sku)
            .IsUnique();

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Many-to-many
        builder.HasMany(p => p.Tags)
            .WithMany(t => t.Products)
            .UsingEntity(j => 
                j.ToTable("ProductTags"));
    }
}
```

</div>
<div>

### Applying Configuration
```csharp
protected override void OnModelCreating(
    ModelBuilder modelBuilder)
{
    // Single configuration
    modelBuilder.ApplyConfiguration(
        new ProductConfiguration());

    // All from assembly
    modelBuilder
        .ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
}
```

### Benefits
- ✅ Separation of concerns
- ✅ Reusable configurations
- ✅ Cleaner DbContext
- ✅ Easier maintenance
- ✅ Better organization

### Value Conversions
```csharp
builder.Property(p => p.Status)
    .HasConversion<string>();

builder.Property(p => p.Tags)
    .HasConversion(
        v => JsonSerializer.Serialize(v),
        v => JsonSerializer
            .Deserialize<List<string>>(v)
    );
```

</div>
</div>

---

# Connection Strings & Configuration

Setting up database connections in ASP.NET Core

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyApp;Trusted_Connection=true;",
    "PostgreSQL": "Host=localhost;Database=myapp;Username=user;Password=pass"
  }
}
```

### Program.cs Configuration
```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        
        sqlOptions.CommandTimeout(60);
        sqlOptions.MigrationsAssembly("MyApp.Data");
    });

    // Development settings only
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }

    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
```

---

# Advanced Configuration Options

<div class="grid grid-cols-2 gap-4">
<div>

### DbContext Pooling
```csharp
// More efficient for web apps
builder.Services.AddDbContextPool
    <AppDbContext>(
    options => options.UseSqlServer(
        connectionString),
    poolSize: 128);
```

### Connection Resiliency
```csharp
options.UseSqlServer(
    connectionString,
    sqlOptions => 
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: 
                TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        );
    }
);
```

</div>
<div>

### Multiple Contexts
```csharp
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(
        connectionString1));

builder.Services.AddDbContext
    <IdentityDbContext>(
    options => options.UseSqlServer(
        connectionString2));

builder.Services.AddDbContext
    <LoggingDbContext>(
    options => options.UseNpgsql(
        connectionString3));
```

### Query Tracking Behavior
```csharp
// Global setting
options.UseQueryTrackingBehavior(
    QueryTrackingBehavior.NoTracking);

// Per-query override
var products = context.Products
    .AsTracking()
    .ToList();
```

</div>
</div>

---

# Performance Tips

Optimizing EF Core queries for better performance

<div class="grid grid-cols-2 gap-4">
<div>

### Use AsNoTracking
```csharp
// For read-only queries
var products = await context.Products
    .AsNoTracking()
    .ToListAsync();
```

### Projections (Select Only What You Need)
```csharp
var names = await context.Products
    .Select(p => p.Name)
    .ToListAsync();
```

### Split Queries
```csharp
// Avoid cartesian explosion
var blogs = await context.Blogs
    .Include(b => b.Posts)
    .Include(b => b.Contributors)
    .AsSplitQuery()
    .ToListAsync();
```

### Compiled Queries
```csharp
private static readonly Func<AppDbContext, 
    int, Task<Product>> _getProductById =
    EF.CompileAsyncQuery(
        (AppDbContext ctx, int id) =>
            ctx.Products.First(p => p.Id == id));

var product = 
    await _getProductById(context, 1);
```

</div>
<div>

### Batch Operations (EF Core 7+)
```csharp
await context.Products
    .Where(p => p.Discontinued)
    .ExecuteDeleteAsync();

await context.Products
    .Where(p => p.CategoryId == 1)
    .ExecuteUpdateAsync(
        s => s.SetProperty(
            p => p.Price, 
            p => p.Price * 1.1m));
```

### Avoid N+1 Queries
```csharp
// ❌ Bad: N+1 queries
foreach (var order in orders)
{
    var customer = await context.Customers
        .FindAsync(order.CustomerId);
}

// ✅ Good: Single query
var orders = await context.Orders
    .Include(o => o.Customer)
    .ToListAsync();
```

### Pagination
```csharp
var page = await context.Products
    .OrderBy(p => p.Id)
    .Skip(pageNumber * pageSize)
    .Take(pageSize)
    .AsNoTracking()
    .ToListAsync();
```

</div>
</div>

---

# Best Practices Summary

<div class="grid grid-cols-2 gap-8">
<div>

## Do's ✅

- ✅ Use async methods (`ToListAsync`, etc.)
- ✅ Apply `AsNoTracking` for read-only queries
- ✅ Use projections to select only needed data
- ✅ Implement repository pattern for complex apps
- ✅ Use migrations for schema changes
- ✅ Enable connection resiliency
- ✅ Use DbContext pooling in web apps
- ✅ Write unit tests with in-memory database
- ✅ Use `Include` to prevent N+1 queries
- ✅ Paginate large result sets

</div>
<div>

## Don'ts ❌

- ❌ Don't call `ToList()` unnecessarily
- ❌ Don't ignore query performance
- ❌ Don't load entire tables into memory
- ❌ Don't use lazy loading carelessly
- ❌ Don't concatenate SQL strings
- ❌ Don't ignore N+1 query problems
- ❌ Don't forget to dispose DbContext
- ❌ Don't use EF for bulk operations (use specialized tools)
- ❌ Don't enable sensitive data logging in production
- ❌ Don't forget to handle transactions properly

</div>
</div>

---

# Thank You!

## Questions?

### Resources
- 📚 [EF Core Documentation](https://docs.microsoft.com/ef/core/)
- 🐙 [GitHub Repository](https://github.com/dotnet/efcore)
- ⚡ [EF Core Performance](https://docs.microsoft.com/ef/core/performance/)
- 🎓 [Microsoft Learn - EF Core](https://learn.microsoft.com/ef/core/)

### Key Takeaways
- Entity Framework simplifies data access with LINQ
- Repository pattern improves testability
- Multiple database providers for flexibility
- In-memory database perfect for unit testing
- Performance optimization is crucial for production apps
