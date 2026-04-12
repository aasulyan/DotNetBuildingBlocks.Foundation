# DotNetBuildingBlocks.Foundation Public API Reference

> Internal architecture reference for the **Foundation** package group.
> Describes the public API surface, package boundaries, dependency rules, and future-consumption guidance.
> Written for use as source context by engineers and LLMs designing future package groups.

---

## 1. Overview

The **Foundation** group is the lowest layer of the DotNetBuildingBlocks ecosystem. It provides the shared contracts, primitives, guard utilities, serialization defaults, and time abstractions that every higher-level group depends on.

### What This Group Solves

| Concern | Package |
|---|---|
| Persistence-neutral entity and audit contracts | Abstractions |
| Precondition enforcement and argument validation | Guards |
| Result types, value objects, strongly typed IDs, paging | Primitives |
| Canonical JSON serialization configuration | Serialization |
| Testable time access and UTC validation | Time |

### What Higher-Level Groups Should Expect

Any package group above Foundation (Errors, Diagnostics, Messaging, Persistence, etc.) may freely reference Foundation packages. Foundation packages never reference upward. Higher-level groups should assume:

- Entity, audit, and soft-delete contracts are defined in **Abstractions**.
- The `Result`/`Error` pattern for expected failures lives in **Primitives**.
- Guard clauses are centralized in **Guards**; they must not be duplicated.
- JSON defaults are centralized in **Serialization**; all packages producing or consuming JSON should use them.
- The `IClock` abstraction for testable time lives in **Time**.

---

## 2. Group Design Principles

### Package Boundary Philosophy

Each Foundation package owns exactly one narrow concern. Packages are independently consumable — a project that only needs guard clauses takes `DotNetBuildingBlocks.Guards` without pulling in serialization or time.

### Dependency Direction

Foundation packages sit at the bottom of the dependency graph. Dependencies flow **upward only**:

```
Higher-level groups (Errors, Diagnostics, …)
        ↓ may depend on
   Foundation packages
        ↓ may depend on
   other Foundation packages (only downward)
```

Within Foundation, only one internal dependency exists: **Primitives → Abstractions**. All other Foundation packages are independent leaf nodes.

### Preferred Public API Kinds

- **Interfaces** for contracts that consumers implement or that enable DI-based testing (`IClock`, `IResult`, `IEntity<TId>`).
- **Records** for immutable value carriers (`Error`, `PagedRequest`, `PagedResult<T>`).
- **Abstract base classes** sparingly, only for identity/equality plumbing (`ValueObject`, `StronglyTypedId<TValue>`).
- **Static utility classes** for stateless precondition checks (`Guard`, `TimeGuard`).
- **Static configuration classes** for shared defaults (`JsonSerializerConfiguration`).
- **Extension methods** for opt-in integration with framework types (`JsonSerializerOptions.ApplyDotNetBuildingBlocksDefaults()`).

### What Must Be Avoided

- Vendor-specific or framework-specific logic (no ASP.NET Core, no EF Core, no Serilog).
- Mutable shared state or ambient context singletons.
- Large surface areas — every public type is a long-term contract.
- Duplicating a concept that already exists in another Foundation package.
- Introducing DI registration helpers at this layer (Foundation packages are too low-level to impose a DI container).

---

## 3. Package Inventory

| Package | Primary Purpose | Ecosystem Role | Main API Kinds | Main Consumers |
|---|---|---|---|---|
| **Abstractions** | Persistence-neutral domain contracts | Root contract layer; almost everything depends on it | Interfaces | Primitives, Persistence, Domain packages |
| **Guards** | Argument precondition validation | Standalone utility; any package may use it | Static methods | All packages, application code |
| **Primitives** | Result types, value objects, IDs, paging | Core domain primitives consumed by service/domain layers | Records, abstract classes, sealed classes | Service layers, CQRS handlers, API layers |
| **Serialization** | Canonical JSON serialization defaults | Single source of truth for JSON configuration | Static classes, extension methods | API layers, messaging, any JSON-producing code |
| **Time** | Testable UTC time access and validation | Replaces direct `DateTime.UtcNow` calls | Interface, sealed class, static guard | All packages needing time, test projects |

---

## 4. Dependency Guidance for This Group

### Internal Dependency Graph

```
Abstractions  (no dependencies)
     ↑
Primitives    (depends on Abstractions)

Guards        (no dependencies)
Serialization (no dependencies)
Time          (no dependencies)
```

### Rules

| Rule | Detail |
|---|---|
| Abstractions depends on nothing | It is the root contract package. |
| Primitives depends only on Abstractions | It consumes `IResult`, `IPagedQuery`, `IHasId<TId>`. |
| Guards, Serialization, Time are independent | They have zero project or package references within Foundation. |
| No Foundation package may reference a higher-level group | Errors, Diagnostics, Persistence, etc. are always above Foundation. |
| Higher-level groups may reference any Foundation package | This is the intended consumption direction. |

### What Future Groups May Consume

- **Errors** group → `Primitives.Errors.Error`, `Primitives.Results.Result`, `Abstractions.IResult`.
- **Persistence** group → `Abstractions.IEntity<TId>`, `Abstractions.IAuditable`, `Abstractions.ISoftDeletable`, `Primitives.Paging.*`, `Primitives.StronglyTypedIds.StronglyTypedId<T>`.
- **Messaging / API** groups → `Serialization.JsonSerializerConfiguration`, `Primitives.Results.Result<T>`, `Primitives.Paging.PagedResult<T>`.
- **Any group** → `Guards.Guard`, `Time.IClock`, `Time.TimeGuard`.

### Pure vs Integration

All five Foundation packages are **pure/generic**. None contain vendor-specific or framework-specific logic. Integration packages (e.g., `DotNetBuildingBlocks.Serilog`) belong in dedicated higher-level groups.

---

## 5. Package-by-Package Reference

---

### Package: DotNetBuildingBlocks.Abstractions

#### Purpose

Provides the minimal, persistence-neutral interfaces that define what an identifiable entity, an auditable object, a soft-deletable object, a paged query, and a result look like — without prescribing any implementation.

#### What Belongs Here

- Marker and behavioral interfaces for domain and persistence concepts.
- Contracts that multiple higher-level packages need to share.
- Interfaces that enable DI-based polymorphism across layers.

#### What Must Not Belong Here

- Concrete implementations of these interfaces (those belong in Primitives or higher).
- Anything tied to a persistence framework (EF Core, Dapper, Mongo).
- Validation, guard, or exception logic.
- Models, records, or classes with behavior.

#### Public API Shape

| Category | Present |
|---|---|
| Interfaces | Yes — core contract layer |
| Models / Records | No |
| Options classes | No |
| Extension methods | No |
| Static helpers | No |
| DI registration | No |

#### Key Public Types

| Type | Role | Future Reuse |
|---|---|---|
| `IHasId<out TId>` | Minimal identity contract. Single property: `TId Id { get; }`. Covariant. | Any package that needs to identify an object by ID without coupling to a persistence layer. |
| `IEntity<out TId>` | Entity contract. Extends `IHasId<TId>`. Covariant. | Persistence packages, repository abstractions, domain model base classes. |
| `IAuditable` | Audit-trail contract. Exposes `CreatedAtUtc`, `CreatedBy`, `LastModifiedAtUtc`, `LastModifiedBy`. All timestamps are `DateTimeOffset` in UTC. | Persistence interceptors, audit-log middleware, domain base classes. |
| `ISoftDeletable` | Logical-delete contract. Exposes `IsDeleted`, `DeletedAtUtc`, `DeletedBy`. | Persistence query filters, soft-delete interceptors. |
| `IPagedQuery` | Read-only paging parameters. Exposes `PageNumber` (1-based) and `PageSize`. | Query handlers, repository methods, API request models. |
| `IResult` | Minimal success/failure contract. `IsSuccess` and computed `IsFailure`. | The `Result` class in Primitives implements this. Any code that needs a non-generic result contract. |

#### Public API Usage Patterns

A domain entity implements the Abstractions interfaces:

```
public class Order : IEntity<Guid>, IAuditable, ISoftDeletable { … }
```

A query handler accepts `IPagedQuery`:

```
Task<PagedResult<OrderDto>> Handle(GetOrdersQuery query)
    where GetOrdersQuery : IPagedQuery
```

A service returns `IResult`:

```
IResult Validate(Order order);
```

Consumers reference `DotNetBuildingBlocks.Abstractions` and code against interfaces. Concrete types come from `Primitives` or application code.

#### Package Boundaries

- Abstractions defines **contracts only**. Primitives provides **concrete implementations** of those contracts.
- Abstractions never references Primitives or any other Foundation package.

#### Future Reuse Guidance

- **Must reuse:** `IEntity<TId>`, `IAuditable`, `ISoftDeletable` — never re-define entity or audit interfaces in higher-level packages.
- **Must reuse:** `IResult` — the result contract lives here; result implementations live in Primitives.
- **Must reuse:** `IPagedQuery` — paging request shape is standardized here.
- Future packages that need a new cross-cutting contract (e.g., `ITenantScoped`) should evaluate whether it belongs in Abstractions.

---

### Package: DotNetBuildingBlocks.Guards

#### Purpose

Provides a single static `Guard` class with fluent precondition-check methods that validate method arguments and throw standard .NET exceptions (`ArgumentNullException`, `ArgumentException`, `ArgumentOutOfRangeException`) on violation.

#### What Belongs Here

- Stateless argument-validation methods that return the validated value.
- Overloads for common .NET types (`string`, `Guid`, `int`, `long`, `decimal`, collections).
- Use of `[CallerArgumentExpression]` to auto-capture parameter names.

#### What Must Not Belong Here

- Domain-specific validation rules (those belong in the Validation package in the Errors group).
- Business-rule validation or error accumulation.
- Time-specific guards (`TimeGuard` lives in the Time package).
- Exception types or custom exception hierarchies.

#### Public API Shape

| Category | Present |
|---|---|
| Static utility class | Yes — `Guard` |
| Extension methods | No |
| Interfaces | No |
| DI registration | No |

#### Key Public Types

| Type | Role | Future Reuse |
|---|---|---|
| `Guard` | Static class. All methods follow the pattern `Guard.AgainstX(value, paramName?)` and return the validated value for inline assignment. | Every package in the ecosystem should use `Guard` for argument preconditions instead of hand-writing null checks or range checks. |

**Method inventory (all static on `Guard`):**

| Method | Validates |
|---|---|
| `AgainstNull<T>(T?, …) where T : class` | Reference type not null |
| `AgainstNull<T>(T?, …) where T : struct` | Nullable value type not null |
| `AgainstNullOrEmpty(string?, …)` | String not null or empty |
| `AgainstNullOrWhiteSpace(string?, …)` | String not null, empty, or whitespace |
| `AgainstDefault<T>(T, …) where T : struct` | Value type not default |
| `AgainstEmpty(Guid, …)` | Guid not `Guid.Empty` |
| `AgainstNullOrEmpty<T>(IReadOnlyCollection<T>?, …)` | Collection not null or empty |
| `AgainstNullOrEmpty<T>(IEnumerable<T>?, …)` | Enumerable not null or empty |
| `AgainstOutOfRange(int/long/decimal, min, max, …)` | Numeric value within inclusive range |
| `AgainstNegative(int/long/decimal, …)` | Numeric value ≥ 0 |
| `AgainstZeroOrNegative(int/long/decimal, …)` | Numeric value > 0 |

#### Public API Usage Patterns

Inline validation with return-value chaining:

```
public Order(Guid id, string name, int quantity)
{
    Id = Guard.AgainstEmpty(id);
    Name = Guard.AgainstNullOrWhiteSpace(name);
    Quantity = Guard.AgainstZeroOrNegative(quantity);
}
```

The `[CallerArgumentExpression]` attribute means the caller does not need to pass the parameter name as a string — it is captured automatically.

#### Package Boundaries

- **Guards** handles low-level argument preconditions and throws standard .NET argument exceptions.
- **Validation** (in the Errors group) handles business-rule validation and accumulates errors into a result.
- **TimeGuard** (in the Time package) handles UTC-specific time validation. This is intentional — time guards live close to the time abstraction.

#### Future Reuse Guidance

- **Must reuse:** All argument validation in DotNetBuildingBlocks packages must use `Guard` instead of manual `if (x == null) throw` patterns.
- **Must not duplicate:** Higher-level packages must not create their own guard utilities.
- If a new kind of guard is needed (e.g., `AgainstNullOrEmpty` for `ReadOnlySpan<T>`), it should be added to this package.

---

### Package: DotNetBuildingBlocks.Primitives

#### Purpose

Provides the concrete primitive types that the rest of the ecosystem builds on: the `Result`/`Error` pattern for representing expected operation outcomes, `ValueObject` for domain value equality, `StronglyTypedId<T>` for type-safe identifiers, and `PagedRequest`/`PagedResult<T>` for pagination.

#### What Belongs Here

- Concrete implementations of `IResult`.
- The `Error` record used across the result pattern.
- Base types for value objects and strongly typed identifiers.
- Paging request and response models.

#### What Must Not Belong Here

- Exception types or exception hierarchies (those belong in the Errors group).
- HTTP-specific error models or ProblemDetails (those belong in the Errors group).
- Persistence-specific implementations.
- Serialization logic.

#### Public API Shape

| Category | Present |
|---|---|
| Records | Yes — `Error`, `PagedRequest`, `PagedResult<T>` |
| Sealed classes | Yes — `Result<TValue>` |
| Classes | Yes — `Result` |
| Abstract classes | Yes — `ValueObject` |
| Abstract records | Yes — `StronglyTypedId<TValue>` |
| Static factory methods | Yes |
| Interfaces consumed | `IResult`, `IPagedQuery`, `IHasId<TId>` (from Abstractions) |

#### Key Public Types

**Errors sub-namespace (`DotNetBuildingBlocks.Primitives.Errors`)**

| Type | Role | Future Reuse |
|---|---|---|
| `Error` | Sealed record with `Code` and `Message` properties. Static `None` field for the absence of an error. Factory method `Error.Create(code, message)` with validation. | Every package that needs to represent an expected failure should use this `Error` record. The Errors group builds exception-to-error and error-to-ProblemDetails translation on top of this type. |

**Results sub-namespace (`DotNetBuildingBlocks.Primitives.Results`)**

| Type | Role | Future Reuse |
|---|---|---|
| `Result` | Non-generic result. Implements `IResult`. Exposes `IsSuccess`, `IsFailure`, `Error`. Static factories: `Result.Success()`, `Result.Failure(error)`, `Result.Success<TValue>(value)`, `Result.Failure<TValue>(error)`. | CQRS command handlers, service methods, domain operations — any method that can succeed or fail without throwing. |
| `Result<TValue>` | Generic result. Sealed, inherits `Result`. Adds `TValue? Value`. Static factories: `Result<TValue>.Success(value)`, `Result<TValue>.Failure(error)`. | Query handlers, service methods returning data. |

**ValueObjects sub-namespace (`DotNetBuildingBlocks.Primitives.ValueObjects`)**

| Type | Role | Future Reuse |
|---|---|---|
| `ValueObject` | Abstract base class. Implements `IEquatable<ValueObject>`. Subclasses override `GetEqualityComponents()` to define structural equality. Provides `==`, `!=`, `Equals`, `GetHashCode`. | Domain value objects (e.g., `Money`, `Address`, `EmailAddress`) inherit from this to get correct equality semantics. |

**StronglyTypedIds sub-namespace (`DotNetBuildingBlocks.Primitives.StronglyTypedIds`)**

| Type | Role | Future Reuse |
|---|---|---|
| `StronglyTypedId<TValue>` | Abstract record wrapping a `TValue` (where `TValue : notnull`). Sealed `ToString()`. Record equality based on `Value`. | Domain and persistence layers define IDs as `public sealed record OrderId(Guid Value) : StronglyTypedId<Guid>(Value);` to prevent primitive obsession. |

**Paging sub-namespace (`DotNetBuildingBlocks.Primitives.Paging`)**

| Type | Role | Future Reuse |
|---|---|---|
| `PagedRequest` | Sealed record implementing `IPagedQuery`. Defaults: `PageNumber = 1`, `PageSize = 20`. Computed `Skip` property. | API request models, query objects. Any package that accepts paging parameters should accept `IPagedQuery` or use `PagedRequest` directly. |
| `PagedResult<T>` | Sealed record. `Items`, `PageNumber`, `PageSize`, `TotalCount`, computed `TotalPages`, `HasPreviousPage`, `HasNextPage`. Static `Empty(pageNumber, pageSize)` factory. | Repository return types, API response models. The single canonical paged response in the ecosystem. |

#### Public API Usage Patterns

**Result pattern:**

```
Result<Order> result = orderService.CreateOrder(command);

if (result.IsFailure)
    // inspect result.Error.Code, result.Error.Message
```

**Value object:**

```
public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

**Strongly typed ID:**

```
public sealed record OrderId(Guid Value) : StronglyTypedId<Guid>(Value);
```

**Paging:**

```
var request = new PagedRequest(PageNumber: 2, PageSize: 50);
PagedResult<OrderDto> result = await repository.GetOrders(request);
// result.TotalPages, result.HasNextPage, etc.
```

#### Package Boundaries

- **Primitives** provides concrete types. **Abstractions** provides the interfaces they implement.
- **Primitives.Errors.Error** is a value carrier. Exception types and HTTP error translation belong in the Errors group.
- **Primitives.Paging** defines the data shapes. Query execution and database-level paging belong in persistence packages.

#### Future Reuse Guidance

- **Must reuse:** `Error`, `Result`, `Result<T>` — never create alternative result/error types in other packages.
- **Must reuse:** `PagedRequest`, `PagedResult<T>` — never create alternative paging models.
- **Must reuse:** `ValueObject`, `StronglyTypedId<T>` — never create alternative base types for these patterns.
- If future packages need to extend results (e.g., with validation errors), they should compose with `Error` rather than replacing it.

---

### Package: DotNetBuildingBlocks.Serialization

#### Purpose

Provides a single canonical JSON serialization configuration for the ecosystem, built on `System.Text.Json`. Ensures all DotNetBuildingBlocks packages and consuming applications serialize JSON consistently.

#### What Belongs Here

- Default `JsonSerializerOptions` configuration.
- Constants for serialization defaults.
- Extension methods to apply defaults to existing `JsonSerializerOptions`.

#### What Must Not Belong Here

- Custom `JsonConverter` implementations for specific domain types (those belong in the package that owns the type).
- XML, MessagePack, Protobuf, or other serialization formats.
- HTTP-specific serialization setup (that belongs in API integration packages).
- Vendor-specific serialization (e.g., Newtonsoft.Json).

#### Public API Shape

| Category | Present |
|---|---|
| Static classes | Yes — `JsonSerializerDefaults`, `JsonSerializerConfiguration` |
| Extension methods | Yes — on `JsonSerializerOptions` |
| Constants | Yes |
| Interfaces | No |
| DI registration | No |

#### Key Public Types

| Type | Role | Future Reuse |
|---|---|---|
| `JsonSerializerDefaults` | Static class exposing named constants: `PropertyNameCaseInsensitive = true`, `WriteIndented = false`. | Reference when explaining or documenting the default choices. |
| `JsonSerializerConfiguration` | Static class. `Default` property returns a shared read-only `JsonSerializerOptions` instance. `CreateDefault()` returns a new mutable instance. `ApplyDefaults(options)` mutates an existing instance. | Any package or application that needs canonical JSON options. |
| `JsonSerializerConfigurationExtensions` | Extension methods on `JsonSerializerOptions`: `ApplyDotNetBuildingBlocksDefaults()` (mutates and returns), `CreateDotNetBuildingBlocksDefaults()` (static convenience). | Fluent configuration in `Startup` / `Program.cs` or in test setup. |

**Applied defaults:**

| Setting | Value |
|---|---|
| `PropertyNamingPolicy` | `JsonNamingPolicy.CamelCase` |
| `DictionaryKeyPolicy` | `JsonNamingPolicy.CamelCase` |
| `PropertyNameCaseInsensitive` | `true` |
| `DefaultIgnoreCondition` | `JsonIgnoreCondition.WhenWritingNull` |
| `WriteIndented` | `false` |
| Converters | `JsonStringEnumConverter` with `CamelCase` |

#### Public API Usage Patterns

**Use shared defaults:**

```
var json = JsonSerializer.Serialize(order, JsonSerializerConfiguration.Default);
```

**Create a new configured instance:**

```
var options = JsonSerializerConfiguration.CreateDefault();
options.WriteIndented = true; // override one setting
```

**Apply defaults to existing options:**

```
var options = new JsonSerializerOptions();
options.ApplyDotNetBuildingBlocksDefaults();
```

#### Package Boundaries

- **Serialization** owns the JSON configuration shape. It does not own converters for specific domain types.
- Custom converters for `StronglyTypedId<T>` or `Error` should live in the package that owns those types or in a dedicated converter package.

#### Future Reuse Guidance

- **Must reuse:** `JsonSerializerConfiguration.Default` or `ApplyDotNetBuildingBlocksDefaults()` — never create parallel JSON defaults.
- If a future package needs to add a custom converter, it should start from `CreateDefault()` or call `ApplyDotNetBuildingBlocksDefaults()` and then append converters.
- If the ecosystem later needs a non-JSON serialization standard (e.g., MessagePack), that belongs in a separate package (e.g., `DotNetBuildingBlocks.Serialization.MessagePack`), not in this package.

---

### Package: DotNetBuildingBlocks.Time

#### Purpose

Provides a testable abstraction over system time (`IClock`) and UTC-enforcement guard methods (`TimeGuard`). Eliminates direct `DateTime.UtcNow` calls across the ecosystem, enabling deterministic testing and consistent UTC discipline.

#### What Belongs Here

- The `IClock` interface and its production implementation `SystemClock`.
- UTC-enforcement guard methods.

#### What Must Not Belong Here

- Time zone conversion utilities (those belong in a dedicated package if needed).
- Scheduling, cron, or timer logic.
- Date formatting or parsing utilities.
- NodaTime or other third-party time library wrappers.

#### Public API Shape

| Category | Present |
|---|---|
| Interfaces | Yes — `IClock` |
| Sealed classes | Yes — `SystemClock` |
| Static utility classes | Yes — `TimeGuard` |
| DI registration | No (consumers register `IClock` in their own composition root) |

#### Key Public Types

| Type | Role | Future Reuse |
|---|---|---|
| `IClock` | Interface. Properties: `DateTime UtcNow`, `DateTimeOffset UtcNowOffset`, `DateOnly TodayUtc`. | Every package or service that needs the current time must depend on `IClock` instead of calling `DateTime.UtcNow` directly. Test projects provide a fake `IClock`. |
| `SystemClock` | Sealed production implementation of `IClock`. Delegates to `DateTime.UtcNow` / `DateTimeOffset.UtcNow`. | Registered in DI as `IClock` in application composition roots. |
| `TimeGuard` | Static class. `AgainstNonUtc(DateTime, …)` — throws `ArgumentException` if `Kind != DateTimeKind.Utc`. `AgainstNonUtc(DateTimeOffset, …)` — throws if `Offset != TimeSpan.Zero`. | Any code that accepts a `DateTime` or `DateTimeOffset` and needs to enforce UTC. Persistence interceptors, event publishers, audit stampers. |

#### Public API Usage Patterns

**Register in DI:**

```
services.AddSingleton<IClock, SystemClock>();
```

**Inject and use:**

```
public class OrderService(IClock clock)
{
    public Order Create(…)
    {
        var order = new Order { CreatedAtUtc = clock.UtcNowOffset };
        …
    }
}
```

**Enforce UTC at boundaries:**

```
public void SetDeadline(DateTimeOffset deadline)
{
    TimeGuard.AgainstNonUtc(deadline);
    _deadline = deadline;
}
```

**Test with a fake clock:**

```
var fakeClock = new FakeClock(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));
var service = new OrderService(fakeClock);
```

#### Package Boundaries

- **Time** owns the clock abstraction and UTC guards. It does not own general argument guards (those are in Guards).
- `TimeGuard` is intentionally separate from `Guard` because it is domain-specific to time and lives next to the `IClock` contract.

#### Future Reuse Guidance

- **Must reuse:** `IClock` — never call `DateTime.UtcNow` or `DateTimeOffset.UtcNow` directly in reusable packages.
- **Must reuse:** `TimeGuard.AgainstNonUtc` — never write manual UTC-kind checks.
- If a future package needs `TimeProvider` (.NET 8) integration, it should either extend `IClock` or provide an adapter in a separate package — not replace `IClock`.
- `FakeClock` / `TestClock` implementations are intentionally left to test projects or a future `DotNetBuildingBlocks.Time.Testing` package.

---

## 6. Cross-Package Design Map

### Ownership Boundaries

```
┌────────────────────────────────────────────────────────────┐
│                     Foundation Group                        │
│                                                            │
│  Abstractions ──────── Contracts (interfaces only)         │
│       ↑                                                    │
│  Primitives ────────── Concrete types implementing         │
│                        Abstractions contracts              │
│                                                            │
│  Guards ────────────── Argument precondition checks        │
│                                                            │
│  Serialization ─────── JSON configuration defaults         │
│                                                            │
│  Time ──────────────── Clock abstraction + UTC guards      │
└────────────────────────────────────────────────────────────┘
```

### Composition Flow

1. **Abstractions** defines the shapes (`IResult`, `IEntity<TId>`, `IAuditable`, `ISoftDeletable`, `IPagedQuery`, `IHasId<TId>`).
2. **Primitives** implements those shapes (`Result`, `Error`, `PagedRequest`, `PagedResult<T>`) and adds new primitives (`ValueObject`, `StronglyTypedId<T>`).
3. **Guards**, **Serialization**, and **Time** are independent utilities — they do not depend on Abstractions or Primitives, and they do not depend on each other.

### How Consumers Choose

| Need | Package |
|---|---|
| "I need to define a domain entity" | Abstractions (interface) + Primitives (StronglyTypedId for ID) |
| "I need to return success or failure from a method" | Primitives (Result / Error) |
| "I need to validate a method argument" | Guards |
| "I need to validate a DateTime is UTC" | Time (TimeGuard) |
| "I need the current time" | Time (IClock) |
| "I need to serialize JSON" | Serialization |
| "I need a paged response" | Primitives (PagedResult\<T\>) |
| "I need structural equality for a value type" | Primitives (ValueObject) |

---

## 7. Public API Stability Guidance

### Long-Term Stable Contracts

The following are highest-stability APIs and must not change without a major version bump:

- All interfaces in **Abstractions** (`IEntity<TId>`, `IAuditable`, `ISoftDeletable`, `IPagedQuery`, `IResult`, `IHasId<TId>`).
- The `Error` record shape (`Code`, `Message`).
- The `Result` / `Result<TValue>` class shapes and factory methods.
- The `IClock` interface.
- The `Guard.AgainstX` method signatures.

### Intentionally Small Surfaces

- **Abstractions** should remain a handful of interfaces. Resist adding utility methods or extension methods here.
- **Guards** should only add new `AgainstX` methods, never change existing ones.
- **Time** should remain three types only (`IClock`, `SystemClock`, `TimeGuard`).

### API Design Preferences

| Approach | When to Use |
|---|---|
| Interface-based | Cross-cutting contracts consumed via DI (`IClock`, `IResult`) |
| Record-based | Immutable value carriers (`Error`, `PagedRequest`, `PagedResult<T>`) |
| Extension-method-based | Optional integration with framework types (`JsonSerializerOptions`) |
| Static-method-based | Stateless utilities with no DI need (`Guard`, `TimeGuard`) |
| Abstract-class-based | Only when equality/identity plumbing must be inherited (`ValueObject`, `StronglyTypedId<T>`) |

### What to Avoid

- Adding optional parameters to existing methods (use new overloads instead).
- Changing record property types.
- Adding required interface members (this is a binary-breaking change).
- Making sealed types unsealed.
- Exposing internal helper types.

---

## 8. Guidance for Future Package Authors

### What Already Exists — Do Not Duplicate

| Concept | Canonical Location |
|---|---|
| Entity identity | `Abstractions.IHasId<TId>`, `Abstractions.IEntity<TId>` |
| Audit trail | `Abstractions.IAuditable` |
| Soft delete | `Abstractions.ISoftDeletable` |
| Paged query parameters | `Abstractions.IPagedQuery`, `Primitives.Paging.PagedRequest` |
| Paged responses | `Primitives.Paging.PagedResult<T>` |
| Result/Error pattern | `Primitives.Results.Result`, `Primitives.Results.Result<T>`, `Primitives.Errors.Error` |
| Value object equality | `Primitives.ValueObjects.ValueObject` |
| Strongly typed IDs | `Primitives.StronglyTypedIds.StronglyTypedId<T>` |
| Argument guards | `Guards.Guard` |
| UTC time access | `Time.IClock`, `Time.SystemClock` |
| UTC validation | `Time.TimeGuard` |
| JSON defaults | `Serialization.JsonSerializerConfiguration` |

### What to Import and Reuse

- Reference `DotNetBuildingBlocks.Abstractions` whenever you need entity, audit, or result contracts.
- Reference `DotNetBuildingBlocks.Primitives` whenever you need concrete result types, error records, paging, value objects, or strongly typed IDs.
- Reference `DotNetBuildingBlocks.Guards` in any package that validates arguments.
- Reference `DotNetBuildingBlocks.Time` in any package that needs current time or UTC enforcement.
- Reference `DotNetBuildingBlocks.Serialization` in any package that produces or consumes JSON.

### What to Keep Decoupled

- Higher-level packages must not force Foundation packages to reference them.
- Framework-specific packages (e.g., ASP.NET Core integration) must not live inside Foundation.
- Custom `JsonConverter` implementations for domain types should not be added to the Serialization package.
- Test fakes for `IClock` should not live in the production Time package.

### How to Safely Extend

- To add a new error category → compose with `Error` from Primitives, do not create a parallel error type.
- To add a new guard method → submit it to Guards, do not create a package-local guard class.
- To add a new paging strategy → extend `PagedResult<T>` consumption patterns, do not create a competing paged response type.
- To add a vendor-specific clock → create a separate integration package that adapts `IClock`, do not modify the Time package.

---

## 9. Suggested Consumer Mental Model

A .NET developer using DotNetBuildingBlocks should think of the Foundation group as **the ground floor**:

| Mental shortcut | Package |
|---|---|
| "What shape is an entity?" | → Abstractions |
| "How do I represent success or failure?" | → Primitives |
| "How do I enforce a precondition?" | → Guards |
| "How do I get the current time in a testable way?" | → Time |
| "How should I configure JSON serialization?" | → Serialization |

In day-to-day development:

- You reference **Abstractions** in your domain layer and shared contracts.
- You reference **Primitives** in your service/application layer for results, errors, paging, value objects, and typed IDs.
- You use **Guards** everywhere you validate input arguments.
- You inject **IClock** instead of calling `DateTime.UtcNow`.
- You call `JsonSerializerConfiguration.Default` or `ApplyDotNetBuildingBlocksDefaults()` when configuring JSON.

These packages are small, stable, and predictable. They should feel invisible in daily use — they solve common problems once so you never think about them again.

---

## 10. Final Summary

The **Foundation** group is the bedrock of the DotNetBuildingBlocks ecosystem. It provides:

- **Abstractions** — six clean interfaces defining entity identity, auditing, soft delete, paging, and operation results.
- **Primitives** — concrete result/error types, value-object and strongly-typed-ID base classes, and paging models.
- **Guards** — a single static class with 18 argument-validation methods covering nulls, empties, ranges, and numeric constraints.
- **Serialization** — canonical `System.Text.Json` configuration with camelCase, null-ignoring, and string-enum defaults.
- **Time** — a testable clock interface, its production implementation, and UTC-enforcement guards.

Every higher-level package group — Errors, Diagnostics, Persistence, Messaging, API — builds on top of Foundation. The group's public API is intentionally small, stable, and vendor-agnostic. Future package authors should consume Foundation contracts and types rather than duplicating them, and they should respect the dependency direction: Foundation depends on nothing above it.
