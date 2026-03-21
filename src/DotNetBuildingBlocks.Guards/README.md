# DotNetBuildingBlocks.Guards

## Purpose

`DotNetBuildingBlocks.Guards` provides lightweight, reusable guard clauses for validating arguments and protecting invariants in constructors, factory methods, and other low-level APIs.

This package is intentionally small. It focuses on programmer errors and invalid input checks and does not replace `Result`-based expected failure flows from other foundation packages.

## Features

- Guard against `null`
- Guard against `null`, empty, and whitespace strings
- Guard against default values
- Guard against empty `Guid`
- Guard against null or empty collections
- Guard against out-of-range numeric values
- Guard against negative and zero-or-negative values
- Preserves argument names
- Uses readable exception messages
- Supports `CallerArgumentExpression` for natural call sites

## Installation

```bash
dotnet add package DotNetBuildingBlocks.Guards
