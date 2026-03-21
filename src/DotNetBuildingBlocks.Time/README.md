# DotNetBuildingBlocks.Time

## Purpose

`DotNetBuildingBlocks.Time` provides small, reusable, UTC-safe time abstractions for low-level .NET building blocks.

The package focuses on predictable access to current time and on test-friendly design. It avoids hidden global state and keeps usage dependency injection friendly.

## Features

- `IClock` abstraction
- `SystemClock` implementation
- UTC-based current time access
- UTC date helper
- UTC validation helper
- Small and framework-neutral API surface
- Easy unit testing with fake clocks in test projects

## Installation

```bash
dotnet add package DotNetBuildingBlocks.Time
