# DotNetBuildingBlocks

DotNetBuildingBlocks is a reusable .NET Building Blocks ecosystem for microservices and distributed systems. The goal is to provide small, focused, independently versionable NuGet packages that can be combined package by package instead of forcing every service into a single monolith-style shared library.

This repository currently contains the **Foundation** solution only.

## Foundation solution

Solution name:

- `DotNetBuildingBlocks.Foundation.sln`

Included packages:

- `DotNetBuildingBlocks.Abstractions`
- `DotNetBuildingBlocks.Primitives`
- `DotNetBuildingBlocks.Guards`
- `DotNetBuildingBlocks.Time`
- `DotNetBuildingBlocks.Serialization`

Each package has its own unit test project and README.

## Package responsibilities

### DotNetBuildingBlocks.Abstractions
Owns only very small, provider-neutral contracts and marker abstractions that other low-level packages can rely on safely.

### DotNetBuildingBlocks.Primitives
Owns low-level reusable types such as `Error`, `Result`, `Result<T>`, paging models, value object support, and strongly typed id support.

### DotNetBuildingBlocks.Guards
Owns guard clauses and invariant helpers for constructor validation, factory validation, and method preconditions.

### DotNetBuildingBlocks.Time
Owns UTC-safe time abstractions such as `IClock`, `SystemClock`, and small time helper extensions.

### DotNetBuildingBlocks.Serialization
Owns shared `System.Text.Json` defaults, JSON converter infrastructure, and reusable serialization options.

## Dependency philosophy

Foundation packages stay low-level and must not depend on web hosts, middleware, EF Core, messaging adapters, logging providers, or vendor-specific integrations.

Current dependency flow:

- `DotNetBuildingBlocks.Abstractions` -> none
- `DotNetBuildingBlocks.Primitives` -> `DotNetBuildingBlocks.Abstractions`
- `DotNetBuildingBlocks.Guards` -> none
- `DotNetBuildingBlocks.Time` -> none
- `DotNetBuildingBlocks.Serialization` -> `DotNetBuildingBlocks.Abstractions`, `DotNetBuildingBlocks.Primitives`


## Build

```bash
dotnet restore DotNetBuildingBlocks.Foundation.sln
dotnet build DotNetBuildingBlocks.Foundation.sln -c Release
```

## Test

```bash
dotnet test DotNetBuildingBlocks.Foundation.sln -c Release
```

## Pack

```bash
dotnet pack DotNetBuildingBlocks.Foundation.sln -c Release --no-build -o ./artifacts/packages
```
